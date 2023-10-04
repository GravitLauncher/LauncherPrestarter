namespace WindowsFormsApp1
{
    internal interface IStatusPeporter
    {
        void updateStatus(string status);
        void requestWaitProgressbar();
        void requestNormalProgressbar();
    }
}
