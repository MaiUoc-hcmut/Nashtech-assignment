import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../redux/hooks/redux';
import { fetchCustomers, fetchCustomerInfo } from '../redux/features/customers/customersSlice';
import { Customer } from '../types/globalTypes';
import { Plus, RefreshCw } from 'lucide-react';
import PageHeader from '../components/commons/PageHeader';
import PageFooter from '../components/commons/PageFooter';
import CustomerInfoModal from '../components/customers/CustomerInfoModal';

const CustomerManagement: React.FC = () => {
  const [isPending, setIsPending] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [isOpenInfoModal, setIsOpenInfoModal] = useState(false);
  const [customerToShow, setCustomerToShow] = useState<Customer | null>(null);

  const dispatch = useAppDispatch();
  const { totalCustomers, customers, status, error } = useAppSelector((state) => state.customers);

  // Fetch customers on mount
  useEffect(() => {
    if (status === "idle") {
      dispatch(fetchCustomers(currentPage));
    }

    if (status === "loading") {
      setIsPending(true);
    }

    if (status !== "loading") {
      setIsPending(false);
    }
  }, [dispatch, status, customers]);


  const showCustomerInfo = async (id: number) => {
    try {
      const customer = await dispatch(fetchCustomerInfo(id)).unwrap();
      setCustomerToShow(customer);
      setIsOpenInfoModal(true);
    } catch (error) {
      console.error('Failed to fetch customer info:', error);
    }
  };

  const handleSearch = (value: string) => {
    setSearchTerm(value);
    setCurrentPage(1);
  };

  useEffect(() => {
    dispatch(fetchCustomers(currentPage));
  },[currentPage]);
  
  const totalPages = Math.ceil(totalCustomers / 10);

  return (
    <div className="flex flex-col h-[calc(100vh-20px)] bg-white rounded-lg shadow">
      <div className="p-6">
        <PageHeader
          title="Customer Management"
          searchPlaceholder="Search customers..."
          searchValue={searchTerm}
          onSearch={handleSearch}
          showButton={false}
          buttonIcon={<Plus size={20} />}
        />

        {error && <p className="text-red-500 mb-4">{error}</p>}
      </div>

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
                  <th className="px-4 py-2 text-left">Name</th>
                  <th className="px-4 py-2 text-left">Email</th>
                  <th className="px-4 py-2 text-left">Phone Number</th>
                  <th className="px-4 py-2 text-left">Address</th>
                </tr>
              </thead>
              <tbody>
                {totalCustomers === 0 ? (
                  <tr>
                    <td colSpan={5} className="text-center py-4">No customers found</td>
                  </tr>
                ) : (
                  customers.map((customer) => (
                    <tr 
                      key={customer.id} 
                      className="border-b hover:bg-gray-50 cursor-pointer"
                      onClick={() => showCustomerInfo(customer.id)}
                    >
                      <td className="px-4 py-2">{customer.name}</td>
                      <td className="px-4 py-2">{customer.email}</td>
                      <td className="px-4 py-2">{customer.phoneNumber}</td>
                      <td className="px-4 py-2">{customer.address}</td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>
      

      <div className="mt-auto p-4 border-t">
        <PageFooter
          currentPage={currentPage}
          totalPages={totalPages}
          totalItems={totalCustomers}
          itemsPerPage={10}
          currentItemCount={customers.length}
          onPageChange={setCurrentPage}
          itemLabel="customers"
        />
      </div>

      <CustomerInfoModal 
        isOpen={isOpenInfoModal}
        onClose={
          () => {
            setIsOpenInfoModal(false)
            setCustomerToShow(null)
          }
        }
        customer={customerToShow}
      />
    </div>
  );
};

export default CustomerManagement;