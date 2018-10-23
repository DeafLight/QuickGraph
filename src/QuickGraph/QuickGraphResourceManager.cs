using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;

namespace QuickGraph
{
    public static class QuickGraphResourceManager
    {
        public static Image GetLogo()
        {
            return GetImage("quickgraph");
        }

        public static Image GetBanner()
        {
            return GetImage("quickgraph.banner");
        }

        private static Image GetImage(string name)
        {
            Contract.Requires(name != null);
            using (Stream stream = typeof(QuickGraphResourceManager).Assembly.GetManifestResourceStream(String.Format("QuickGraph.{0}.png", name)))
                return Image.FromStream(stream);
        }

        public static void DumpResources(string path)
        {
            Contract.Requires(path != null);

            GetLogo().Save(Path.Combine(path, "quickgraph.png"), System.Drawing.Imaging.ImageFormat.Png);
            GetBanner().Save(Path.Combine(path, "quickgraph.banner.png"), System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
