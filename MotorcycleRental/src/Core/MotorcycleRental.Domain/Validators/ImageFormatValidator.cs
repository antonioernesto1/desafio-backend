using MotorcycleRental.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Validators
{
    public class ImageFormatValidator
    {
        private static readonly byte[] PNG_SIGNATURE = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte[] BMP_SIGNATURE = { 0x42, 0x4D }; // "BM"

        public static bool IsValidImageFormat(string base64Image, out string? errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(base64Image))
            {
                errorMessage = "Image cannot be null or empty";
                return false;
            }

            try
            {
                var base64Data = base64Image.Contains(",")
                    ? base64Image.Split(',')[1]
                    : base64Image;

                byte[] imageBytes = Convert.FromBase64String(base64Data);

                if (imageBytes.Length >= PNG_SIGNATURE.Length &&
                    imageBytes.Take(PNG_SIGNATURE.Length).SequenceEqual(PNG_SIGNATURE))
                {
                    return true;
                }

                if (imageBytes.Length >= BMP_SIGNATURE.Length &&
                    imageBytes.Take(BMP_SIGNATURE.Length).SequenceEqual(BMP_SIGNATURE))
                {
                    return true;
                }

                errorMessage = "Invalid image format. Only PNG or BPM are allowed";
                return false;
            }
            catch (FormatException)
            {
                errorMessage = "Base64 inválido";
                return false;
            }
        }

        public static ImageFormat GetImageFormat(string base64Image)
        {
            var base64Data = base64Image.Contains(",")
                ? base64Image.Split(',')[1]
                : base64Image;

            byte[] imageBytes = Convert.FromBase64String(base64Data);

            if (imageBytes.Take(PNG_SIGNATURE.Length).SequenceEqual(PNG_SIGNATURE))
                return ImageFormat.PNG;

            if (imageBytes.Take(BMP_SIGNATURE.Length).SequenceEqual(BMP_SIGNATURE))
                return ImageFormat.BMP;

            return ImageFormat.Unknown;
        }
    }

    public enum ImageFormat
    {
        Unknown,
        PNG,
        BMP
    }
}
