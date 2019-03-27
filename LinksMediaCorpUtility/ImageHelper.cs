using System.Drawing;
using System.Drawing.Imaging;

namespace LinksMediaCorpUtility
{
    public class ImageHelper
    {

        /// <summary>
        /// compression image by 50 percent
        /// </summary>
        /// <param name="imagepath"></param>
        /// <param name="savepath"></param>
        public static void VaryQualityLevel(string imagepath, string savepath)
        {
            using (Bitmap bmp1 = new Bitmap(@imagepath))
            {
                EncoderParameters myEncoderParameters = null;
                EncoderParameter myEncoderParameter = null;
                try
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp1.Save(@savepath, jpgEncoder, myEncoderParameters);
                    if (myEncoderParameter != null)
                    {
                        myEncoderParameter.Dispose();
                    }
                    if (myEncoderParameters != null)
                    {
                        myEncoderParameters.Dispose();
                    }
                }
                catch
                {
                    if (myEncoderParameter != null)
                    {
                        myEncoderParameter.Dispose();
                    }
                    if (myEncoderParameters != null)
                    {
                        myEncoderParameters.Dispose();
                    }
                    throw;
                }
            }
        }
        /// <summary>
        /// Get Encoder for image
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            try
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == format.Guid)
                    {
                        return codec;
                    }
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
    }
}