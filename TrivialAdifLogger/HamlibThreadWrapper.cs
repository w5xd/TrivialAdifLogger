using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdifLog
{
    public class HamlibThreadWrapper : IDisposable
    {
        /* Hamlib can block its calling thread for a "while" (I have determined empirically)
         * The specific problem scenario is DigiRite commanding the rig into a split configuration
         * and hamlib blocks its caller until that operation finishes. 
         * 
         * That blocking can delay DigiRite's transmission to beyond the range acceptable to
         * FT8/FT4 decoders. DigiRite, however, has built-in support for configuring how long
         * the user's rig might really need to respond. The solution implemented here
         * is to start hamlib's operation for setting up split, but return to DigiRite 
         * immediately.  */
        private HamLibClr.Rig rig;

        /* while HamLibClr.Rig accessed on its own thread and
        ** assume NOT thread-safe, we access this static method from the
        ** GUI thread because, well, its static */
        public static void listRigs(HamLibClr.RigListItem listDel)
        { HamLibClr.Rig.listRigs(listDel); }

        System.Threading.Thread thread;

        public HamlibThreadWrapper(int rigModelNumber)
        {  
            thread = new System.Threading.Thread(threadHead);
            thread.Start();
            System.Exception failed = null;
            // construct blocks until hamlab returns
            var autoEvent = new AutoResetEvent(false);
            toDo.Add(new ThreadEntry(() =>
                {
                    bool ret = false;
                    try
                    {
                        rig = new HamLibClr.Rig(rigModelNumber);
                        ret = true;
                    }
                    catch (System.Exception e)
                    {
                        failed = e;
                        ret = false;
                    }
                    autoEvent.Set();
                    return ret;
                }
            ));
            autoEvent.WaitOne();
            if (null != failed)
                throw failed;
        }

        delegate bool ThreadEntry(); // ThreadEntry returns false to exit the thread
        System.Collections.Concurrent.BlockingCollection<ThreadEntry> toDo = new System.Collections.Concurrent.BlockingCollection<ThreadEntry>();
        void threadHead()
        {
            for (;;)
            {
                var item = toDo.Take();
                if (!item())
                    break;
            }
        }

        public bool open(String comPort, uint baud)
        {
            bool ret = false;
            // open blocks until hamlib returns
            var autoEvent = new AutoResetEvent(false);
            toDo.Add(() => {
                if (!rig.open(comPort, baud))
                {
                    rig.Dispose();
                    rig = null;
                } else
                    ret = true;
                autoEvent.Set();
                return ret;
            });
            autoEvent.WaitOne();
            return ret;
        }

        public double getFrequency()
        { return rxKhz;  } // return last polled value

        public void setMode(HamLibClr.Mode_t mode)
        {
            toDo.Add(() =>
            {
                rig.setMode(mode);
                return true;
            });
        }

        public void getFrequencyAndMode(ref double  rxKhz, ref double  txKhz,ref HamLibClr.Mode_t  mode,ref bool  split)
        {   // return last polled value
            rxKhz = this.rxKhz;
            txKhz = this.txKhz;
            mode = this.mode;
            split = this.split;
        }

        public bool PTT { set {
                toDo.Add(() =>
                {
                    rig.PTT = value;
                    return true;
                });
                } }

        public void setTransceive(double txrxKhz)
        {
            toDo.Add(() =>
            {
                rig.setTransceive(txrxKhz);
                return true;
            });
        }

        public void setSplit(double rxKhz, double txKhz)
        {
#if DEBUG
            poll(null); // deliberately make the split happen slowly
#endif
            toDo.Add(() =>
            {
                rig.setSplit(rxKhz, txKhz);
                return true;
            });
        }

        double rxKhz = 0;
        double txKhz = 0;
        HamLibClr.Mode_t mode = HamLibClr.Mode_t.MODE_AM;
        bool split = false;

        public delegate void Pollcallback(double rxKhz, double txKhz, HamLibClr.Mode_t mode, bool split);

        public void poll(Pollcallback cb)
        {
            toDo.Add(() =>
            {
                rig.getFrequencyAndMode(ref rxKhz, ref txKhz, ref mode, ref split);
                if (null != cb)
                    cb(rxKhz, txKhz, mode, split);
                return true;
            });
        }

        public void Dispose()
        {
            toDo.Add(() => {
                if (null != rig)
                    rig.Dispose();
                rig = null;
                return false; }
            ); // make thread exit, if running
            if (null != thread)
                thread.Join();
        }

    }
}
