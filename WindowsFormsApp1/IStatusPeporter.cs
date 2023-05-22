using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal interface IStatusPeporter
    {
        void updateStatus(string status);
        void requestWaitProgressbar();
        void requestNormalProgressbar();
    }
}
