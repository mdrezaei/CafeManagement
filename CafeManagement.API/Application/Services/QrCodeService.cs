using QRCoder;

namespace CafeManagement.API.Application.Services
{
    public class QrCodeService
    {
        private readonly string _baseUrl;

        public QrCodeService(IConfiguration configuration)
        {
            _baseUrl = configuration["AppBaseUrl"] ?? "https://localhost:7205";
        }

        public string BuildQrUrl(int tableId)
        {
            return $"{_baseUrl}/menu.html?tableId={tableId}";
        }

        public byte[] GenerateQrCode(string url)
        {
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var pngByteQrCode = new PngByteQRCode(qrCodeData);
            return pngByteQrCode.GetGraphic(20);
        }

    }
}
