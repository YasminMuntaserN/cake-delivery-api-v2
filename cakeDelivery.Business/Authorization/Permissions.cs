namespace cakeDelivery.Business.Authorization;

public enum Permissions
{
    View = 1,
    ManageCakes = 2,
    ManageUsers = 4,
    ManageOrders = 8,
    ManageDeliveries = 16,
    ManageCategories = 32,
    ManageCustomers = 64,
    ManagePayments = 128,
}