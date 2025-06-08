import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../redux/hooks/redux';
import { fetchOrders, getDetailsOrder } from '../redux/features/orders/ordersSlice';
import PageHeader from '../components/commons/PageHeader';
import PageFooter from '../components/commons/PageFooter';
import OrderDetailModal, { OrderDetail } from '../components/orders/DetailsModal';
import { RefreshCw } from 'lucide-react';

const OrdersManagement: React.FC = () => {
  const [isPending, setIsPending] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedOrderDetail, setSelectedOrderDetail] = useState<OrderDetail | null>(null);
  const [modalLoading, setModalLoading] = useState(false);
  const [modalError, setModalError] = useState<string | null>(null);

  const dispatch = useAppDispatch();
  const { totalOrders, orders, status, error } = useAppSelector((state) => state.orders);

  // Fetch orders on mount
  useEffect(() => {
    if (status === "idle") {
      dispatch(fetchOrders());
    }

    if (status === "loading") {
      setIsPending(true);
    }

    if (status !== "loading") {
      setIsPending(false);
    }
  }, [dispatch, status, orders]);

  // Format date for display
  const formatDate = (dateString: string) => {
    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  // Format currency
  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  };

  const handleSearch = (value: string) => {
    setSearchTerm(value);
    setCurrentPage(1); // Reset to first page when searching
  };

  const handleRowClick = async (orderId: number) => {
    setIsModalOpen(true);
    setModalLoading(true);
    setModalError(null);
    setSelectedOrderDetail(null);

    try {
      // Fetch order details from API
      const orderDetail = await dispatch(getDetailsOrder(orderId));
      setSelectedOrderDetail(orderDetail.payload);
    } catch (error) {
      console.error('Error fetching order details:', error);
      setModalError('Failed to load order details');
    } finally {
      setModalLoading(false);
    }
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setSelectedOrderDetail(null);
    setModalError(null);
    setModalLoading(false);
  };

  const totalPages = Math.ceil(totalOrders / 10);

  return (
    <div className="flex flex-col h-[calc(100vh-20px)] bg-white rounded-lg shadow">
      {/* Content area that can grow to push footer down */}
      <div className="flex-1 p-6 rounded-lg shadow">
        <PageHeader
          title="Orders Management"
          searchPlaceholder="Search orders..."
          searchValue={searchTerm}
          onSearch={handleSearch}
          showButton={false}
        />

        {error && <p className="text-red-500 mb-4">{error}</p>}

        <div className="flex-1 overflow-auto">
          {isPending ? (
            <div className="flex justify-center items-center h-32">
              <div className="text-blue-500 flex items-center">
                <RefreshCw size={20} className="animate-spin mr-2" />
                Loading...
              </div>
            </div>
          ) : (
            <div className="flex-1 overflow-auto px-6 pb-6 min-h-0">
              <table className="w-full table-auto">
                <thead>
                  <tr className="bg-gray-100">
                    <th className="px-4 py-2 text-left">Order ID</th>
                    <th className="px-4 py-2 text-left">Customer</th>
                    <th className="px-4 py-2 text-left">Amount</th>
                    <th className="px-4 py-2 text-left">Address</th>
                    <th className="px-4 py-2 text-left">Status</th>
                    <th className="px-4 py-2 text-left">Created</th>
                  </tr>
                </thead>
                <tbody>
                  {orders.length === 0 ? (
                    <tr>
                      <td colSpan={6} className="text-center py-4">No orders found</td>
                    </tr>
                  ) : (
                    orders.map((order) => (
                      <tr 
                        key={order.id} 
                        className="border-b hover:bg-gray-50 cursor-pointer transition-colors"
                        onClick={() => handleRowClick(order.id)}
                      >
                        <td className="px-4 py-2">#{order.id}</td>
                        <td className="px-4 py-2">{order.customer}</td>
                        <td className="px-4 py-2">{formatCurrency(order.amount)}</td>
                        <td className="px-4 py-2">{order.address}</td>
                        <td className="px-4 py-2">
                          <span className={'px-2 py-1 rounded-full text-xs font-medium bg-green-100 text-green-800'}>
                            Success
                          </span>
                        </td>
                        <td className="px-4 py-2">{formatDate(order.createdAt)}</td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      {/* Footer always at the bottom */}
      <div className="mt-auto">
        <PageFooter
          currentPage={currentPage}
          totalPages={totalPages}
          totalItems={orders.length}
          itemsPerPage={10}
          currentItemCount={orders.length}
          onPageChange={setCurrentPage}
          itemLabel="orders"
        />
      </div>

      {/* Order Detail Modal */}
      <OrderDetailModal
        isOpen={isModalOpen}
        onClose={handleCloseModal}
        orderDetail={selectedOrderDetail}
        loading={modalLoading}
        error={modalError}
      />
    </div>
  );
};

export default OrdersManagement;