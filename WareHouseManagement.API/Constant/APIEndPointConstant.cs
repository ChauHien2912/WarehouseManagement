namespace WareHouseManagement.API.Constant
{
    public static class APIEndPointConstant
    {
        static APIEndPointConstant() { }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Role
        {
            public const string RoleEndPoint = ApiEndpoint + "/role";
        }

        public static class Authentication
        {
            public const string AuthenEndPoint = ApiEndpoint + "/authentication";
        }

        public static class Account
        {
            public const string AccountEndPoint = ApiEndpoint + "/account";
        }

        public static class Shipper
        {
            public const string ShipperEndpoint = ApiEndpoint + "/shipper";
        }
        public static class Warehouse
        {
            public const string WarehouseEndpoint = ApiEndpoint + "/warehouses";
        }

        public static class Order
        {
            public const string OrderEndpoint = ApiEndpoint + "/orders";
        }

    }

}
