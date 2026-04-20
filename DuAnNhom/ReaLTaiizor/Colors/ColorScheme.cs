
namespace ReaLTaiizor.Colors
{
    internal class ColorScheme : MaterialColorScheme
    {
        private object blueGrey800;
        private object blueGrey900;
        private object blueGrey500;
        private object lightBlue200;
        private object wHITE;

        public ColorScheme(object blueGrey800, object blueGrey900, object blueGrey500, object lightBlue200, object wHITE)
        {
            this.blueGrey800 = blueGrey800;
            this.blueGrey900 = blueGrey900;
            this.blueGrey500 = blueGrey500;
            this.lightBlue200 = lightBlue200;
            this.wHITE = wHITE;
        }
    }
}