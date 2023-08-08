using Framework.Core;

namespace Orderbox.Core
{
    public class CoreConstant : FrameworkCoreConstant
    {
        public const string DefaultTimeZone = "Singapore Standard Time";

        public static class AgentPrivilegeOption
        {
            public const string Manager = "MANAGER";
            public const string Director = "DIRECTOR";
        }

        public static class AgentStatus
        {
            public const string Activating = "ACTIVATING";
            public const string Active = "ACTIVE";
        }

        public static class CheckoutFormOption
        {
            public const string SimpleInquiry = "SIMPLE_INQUIRY";
        }

        public static class Tenant
        {
            public const string HttpContextTenantKey = "TENANT";
            public const string DefaultLogo = "https://a50-space.sgp1.cdn.digitaloceanspaces.com/obox/assets/img/default-tenant-logo.png";
        }

        public static class Role
        {
            public const string Administrator = "Administrator";
            public const string User = "User";
            public const string Agent = "Agent";
        }

        public static class Product
        {
            public const string DefaultImage = "/img/logo.png";
            public const string DefaultUnit = "pcs";
        }

        public static class OrderStatus
        {
            public const string New = "NEW";
            public const string InProgress = "WIP";
            public const string Sent = "SNT";
            public const string Finished = "FIN";
            public const string Cancelled = "CNL";
        }

        public static class PaymentOption
        {
            public const string Cod = "COD";
            public const string Bank = "BNK";
            public const string Wallet = "WLT";
            public const string PaymentGateway = "PMGW";
        }

        public static class PaymentStatus
        {
            public const string Paid = "PAID";
            public const string Ready = "READY";
            public const string Cancelled = "CANCELLED";
            public const string Expired = "EXPIRED";
        }

        public static class PaymentGatewayProvider
        {
            public const string Xendit = "XENDIT";
        }

        public static class ProductType
        {
            public const string Product = "PRODUCT";
            public const string Voucher = "VOUCHER";
        }

        public static class Bank
        {
            public const string Bca = "BCA";
            public const string Bni = "BNI";
            public const string BpdBali = "BPDBALI";
            public const string Bri = "BRI";
            public const string Btn = "BTN";
            public const string Btpn = "BTPN";
            public const string CimbNiaga = "CIMBNIAGA";
            public const string Danamon = "DANAMON";
            public const string Mandiri = "MANDIRI";
            public const string Mega = "MEGA";
            public const string Maybank = "MAYBANK";
            public const string OcbcNisp = "OCBCNISP";
        }

        public static class Wallet
        {
            public const string Ovo = "OVO";
            public const string Gopay = "GOPAY";
            public const string Dana = "DANA";
        }

        public static class Settings
        {
            public const int DefaultPageSize = 20;
            public const string OrderboxIdPrefix = "OBX";
            public const int MaxProductImage = 10;
        }

        public static class SummaryType
        {
            public const string SummaryInteger = "SUM_INT";
            public const string SummaryMoney = "SUM_MON";
            public const string SummaryItemList = "SUM_ILIST";
        }

        public static class AuthenticationType
        {
            public const string Google = "https://account.google.com";
            public const string Facebook = "FACEBOOK";
        }

        public static class Pagination
        {
            public const int AllPage = -1;
            public const int PageSize = 3;
        }

        public static class DoughnutChart
        {
            public const int MaxMainItemNumber = 3;
        }

        public static class OrderboxClaimTypes
        {
            public const string Issuer = "issuer";
            public const string Picture = "picture";
        }

        public static class RedeemMethod
        {
            public const string Swipe = "SWIPE";
            public const string Admin = "ADMIN";
        }

        public static class VoucherStatus
        {
            public const string Used = "USED";
            public const string Valid = "VALID";
        }
    }
}
