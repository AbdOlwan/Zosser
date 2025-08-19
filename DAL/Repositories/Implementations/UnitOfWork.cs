using DAL.Context;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Interfaces.FAQs;
using DAL.Repositories.Interfaces.Identity;
using DAL.Repositories.Interfaces.Order_Interfaces;
using DAL.Repositories.Interfaces.Payments;
using DAL.Repositories.Interfaces.Payments.Wallets;
using DAL.Repositories.Interfaces.Product_Interfaces;

using DAL.Repositories.Interfaces.Shipments;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        // Repository instances
        public ICustomerRepo Customers { get; }
        public IRefreshTokenRepo RefreshTokenRepo { get; }
        public IProductRepo Products { get; }
        public ICategoryRepo Category { get; }
        public IBrandRepo Brands { get; }

        public IProductImageRepo ProductImages { get; }



       //public IBaseFAQRepo<BaseFAQ> BaseFAQs { get; }
        public ISiteFAQRepo SiteFAQs { get; }
        public IProductFAQRepo ProductFAQs { get; }




        public IOrderRepo Orders { get; }


        public IPaymentRepo Payments { get; }
        public IPaymentCollectionRepo PaymentCollections { get; }
        public IDeliveryAgentRepo DeliveryAgents { get; }

        // Wallet Repositories
        public IWalletRepo Wallets { get; }
        public IWalletTransactionRepo WalletTransactions { get; }
        public IWithdrawalRequestRepo WithdrawalRequests { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            ICustomerRepo customerRepo,

            IRefreshTokenRepo refreshTokenRepo,
            IProductRepo productRepo ,
            ICategoryRepo categoryRepo,
            IBrandRepo brandRepo,
            IProductImageRepo productImageRepo,


            //IBaseFAQRepo<BaseFAQ> baseFAQRepo,
            ISiteFAQRepo siteFAQRepo,
            IProductFAQRepo productFAQRepo,


            IOrderRepo orderRepo,


            IPaymentRepo paymentRepo,
            IPaymentCollectionRepo paymentCollectionRepo,
            IDeliveryAgentRepo deliveryAgentRepo,
            IWalletRepo walletRepo,
            IWalletTransactionRepo walletTransactionRepo,
            IWithdrawalRequestRepo withdrawalRequestRepo

            )
        {
            _context = context;
            Customers = customerRepo;
            RefreshTokenRepo = refreshTokenRepo;
            Products = productRepo;
            Category = categoryRepo;
            Brands = brandRepo;
            ProductImages = productImageRepo;

            //BaseFAQs = baseFAQRepo;
            SiteFAQs = siteFAQRepo;
            ProductFAQs = productFAQRepo;

            Orders = orderRepo;

            Payments = paymentRepo;
            PaymentCollections = paymentCollectionRepo;
            DeliveryAgents = deliveryAgentRepo;
            Wallets = walletRepo;
            WalletTransactions = walletTransactionRepo;
            WithdrawalRequests = withdrawalRequestRepo;

        }



        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
