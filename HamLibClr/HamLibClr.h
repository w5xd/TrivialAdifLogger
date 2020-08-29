#pragma once
struct rig;
typedef struct rig RIG;

namespace HamLibClr {
    /* Minimal wrapper of hamlib for .NET */
    using namespace System;
    public delegate void RigListItem(int model, String^ mfgname, String ^model_name);

    public enum class  Mode_t {MODE_LSB = 1, MODE_USB, MODE_CW, MODE_FM, MODE_AM, MODE_DIG };

	public ref class Rig
	{
    public:
        static void listRigs(RigListItem ^listDel);

        Rig(int modelNumber); 
        ~Rig();
        !Rig();

        bool open(String ^port, unsigned baud);
        bool getFrequencyAndMode(double %rxKhz, double %txKhz, Mode_t %mode, bool %split);
        double getFrequency();
        bool setTransceive(double txrxKhz);
        bool setSplit(double rxKhz, double txKhz);
        bool setMode(Mode_t);
        property bool PTT { bool get(); void set(bool);}

    protected:
        RIG *m_rig;
        int m_digiMode;
	};
}
