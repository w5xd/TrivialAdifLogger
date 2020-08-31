#include "pch.h"
#include <hamlib/rig.h>
#include "HamLibClr.h"

namespace {
    typedef msclr::gcroot<HamLibClr::RigListItem ^> cbHolder_t;
    struct cbData_t {
        cbHolder_t cb;
    };

    int listRigCallback(const rig_caps *caps, void *cbVoid)
    {
        HamLibClr::RigListItem ^cb = ((cbData_t*)cbVoid)->cb;
        cb(caps->rig_model, msclr::interop::marshal_as<System::String ^>(caps->mfg_name),
            msclr::interop::marshal_as<System::String ^>(caps->model_name));
        return -1;
    }
}

namespace HamLibClr {
        void Rig::listRigs(RigListItem ^listDel)
        {
            static bool backendsLoaded = false;
            if (!backendsLoaded)
            {
                backendsLoaded = true;
                rig_load_all_backends();
            }
            cbData_t cbData;
            cbData.cb = listDel;
            rig_list_foreach(&listRigCallback, &cbData);
        }

        // this should never fail
        Rig::Rig(int modelNumber) : m_digiMode(static_cast<int>(RIG_MODE_RTTY))
        {
            m_rig = rig_init(modelNumber);
            if (!m_rig)
                throw gcnew System::Exception(L"Rig initialization failed");
        }

        // but open can and will fail if the serial port won't work and/or the rig isn't there
        bool Rig::open(String ^port, unsigned baud)
        {
            if (!m_rig)
                return false;
            strncpy_s(m_rig->state.rigport.pathname, msclr::interop::marshal_as<std::string>(port).c_str(), FILPATHLEN - 1);
            if (baud)
                m_rig->state.rigport.parm.serial.rate = baud;
            return RIG_OK == rig_open(m_rig);
        }

        double Rig::getFrequency()
        {
            if (!m_rig)
                return -1;
            freq_t cur;
            if (RIG_OK == rig_get_freq(m_rig, RIG_VFO_CURR, &cur))
                return cur * .001; // hamlib does Hz, be we're doing kHz
            return -2;
        }

        bool Rig::getFrequencyAndMode(double %rx, double %tx, Mode_t %mode, bool %split)
        {
            if (!m_rig)
                return false;
            freq_t cur, curtx;
            rmode_t hlmode;
            pbwidth_t hlwidth;
            if (RIG_OK != rig_get_freq(m_rig, RIG_VFO_CURR, &cur))
                return false;
            if (RIG_OK != rig_get_mode(m_rig, RIG_VFO_CURR, &hlmode, &hlwidth))
                return false;
            split_t sp = {};
            vfo_t tx_vfo = 0;
            if (RIG_OK != rig_get_split_vfo(m_rig, RIG_VFO_CURR, &sp, &tx_vfo) ||
                sp != split_t::RIG_SPLIT_ON ||
                RIG_OK != rig_get_freq(m_rig, tx_vfo, &curtx))
                curtx = cur;
            split = cur != curtx;
            rx = cur * .001;    // Hz to kHz
            tx = curtx * .001;
            switch (hlmode)
            {
                case RIG_MODE_LSB:
                    mode = Mode_t::MODE_LSB;
                    break;
                case RIG_MODE_USB:
                    mode = Mode_t::MODE_USB;
                    break;
                case RIG_MODE_CW:
                case RIG_MODE_CWR:
                    mode = Mode_t::MODE_CW;
                    break;
                case RIG_MODE_AM:
                    mode = Mode_t::MODE_AM;
                    break;
                case RIG_MODE_FM:
                    mode = Mode_t::MODE_FM;
                    break;
                case RIG_MODE_RTTY:
                case RIG_MODE_RTTYR:
                case RIG_MODE_PKTLSB:
                case RIG_MODE_PKTUSB:
                    mode = Mode_t::MODE_DIG;
                    m_digiMode = static_cast<int>(hlmode);
                    break;
            }
            return true;
        }

        bool Rig::setTransceive(double txrxKhz)
        {   // turn split OFF
            if (!m_rig)
                return false;
            return RIG_OK == rig_set_split_vfo(m_rig, RIG_VFO_A, split_t::RIG_SPLIT_OFF, RIG_VFO_A) &&
                    RIG_OK == rig_set_freq(m_rig, RIG_VFO_A, txrxKhz * 1000.0);
        }

        bool Rig::setSplit(double rxKhz, double txKhz)
        {
            if (!m_rig)
                return false;
            // match VFOB to current
            if (RIG_OK == rig_set_mode(m_rig, RIG_VFO_B, m_rig->state.current_mode, m_rig->state.current_width))
            {
                if (RIG_OK == rig_set_freq(m_rig, RIG_VFO_A, rxKhz * 1000))
                {   // 1000 is kHz to Hz
                    if (RIG_OK == rig_set_freq(m_rig, RIG_VFO_B, txKhz * 1000))
                    {
                        if (RIG_OK == rig_set_split_vfo(m_rig, RIG_VFO_A, split_t::RIG_SPLIT_ON, RIG_VFO_B))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        bool Rig::setMode(Mode_t m)
        {   // Trivial ADIF logger only really cares about digital modes
            if (!m_rig)
                return false;
            rmode_t rm = static_cast<rmode_t>(m_digiMode);
            switch (m)
            {
            case Mode_t::MODE_LSB:
                rm = RIG_MODE_LSB;
                break;
            case Mode_t::MODE_USB:
                rm = RIG_MODE_USB;
                break;
            case Mode_t::MODE_CW:
                rm = RIG_MODE_CW;
                break;
            case Mode_t::MODE_AM:
                rm = RIG_MODE_AM;
                break;
            case Mode_t::MODE_FM:
                rm = RIG_MODE_FM;
                break;
            }
            return RIG_OK == rig_set_mode(m_rig, RIG_VFO_A, rm, RIG_PASSBAND_NOCHANGE);
        }

        bool Rig::PTT::get()
        {
            ptt_t ptt = ptt_t::RIG_PTT_OFF;
            if (m_rig)
                rig_get_ptt(m_rig, RIG_VFO_A, &ptt);
            return ptt != ptt_t::RIG_PTT_OFF;
        }

        void Rig::PTT::set(bool p)
        {
            if (m_rig)
                rig_set_ptt(m_rig, RIG_VFO_A, p ? ptt_t::RIG_PTT_ON : ptt_t::RIG_PTT_OFF);
        }

        Rig::~Rig()
        { this->!Rig(); }

        Rig::!Rig()
        {
            if (m_rig)
            {
                rig_close(m_rig);
                rig_cleanup(m_rig);
            }
            m_rig = 0;
        }
}