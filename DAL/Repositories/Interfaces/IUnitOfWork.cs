// DAL/Repositories/Interfaces/IUnitOfWork.cs
using DAL.Repositories.Interfaces.FAQs;
using DAL.Repositories.Interfaces.Identity;
using DAL.Repositories.Interfaces.Order_Interfaces;
using DAL.Repositories.Interfaces.Payments;
using DAL.Repositories.Interfaces.Payments.Wallets;
using DAL.Repositories.Interfaces.Product_Interfaces;
using DAL.Repositories.Interfaces.Shipments;

namespace DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Identity Repositories
        ICustomerRepo Customers { get; }
        IRefreshTokenRepo RefreshTokenRepo { get; }


        // Product Repositories
        IProductRepo Products { get; }
        ICategoryRepo Category { get; }
        IBrandRepo Brands { get; }
        IProductImageRepo ProductImages { get; }



        // FAQ Repositories
        ISiteFAQRepo SiteFAQs { get; }
        IProductFAQRepo ProductFAQs { get; }



        // Order Repositories
        IOrderRepo Orders { get; }



        // Payments
        IPaymentRepo Payments { get; }
        IPaymentCollectionRepo PaymentCollections { get; }

        // Shipments
        IDeliveryAgentRepo DeliveryAgents { get; }


        // Wallet Repositories
        IWalletRepo Wallets { get; }
        IWalletTransactionRepo WalletTransactions { get; }
        IWithdrawalRequestRepo WithdrawalRequests { get; }

        /// <summary>
        /// Save all changes to the database
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Save all changes to the database synchronously
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Begin a database transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rollback the current transaction
        /// </summary>
        Task RollbackTransactionAsync();
    }
}