import { useState } from 'react';
import StatsCard from '../components/StatsCard';
import SalesChart from '../components/SalesChart';
import LowStockItems from '../components/LowStockItems';
import RecentOrders from '../components/RecentOrders';
import RecentReviews from '../components/RecentReviews';
import { Stats, SalesData, LowStockItem, RecentOrder, RecentReview } from '../types/dashboardTypes';

const AdminDashboard: React.FC = () => {
  const [stats] = useState<Stats>({
    dailySales: 243,
    pendingOrders: 18,
    lowStock: 7,
    newCustomers: 12,
    totalRevenue: 24560
  });

  const [salesData] = useState<{
    weekly: SalesData[];
    monthly: SalesData[];
    yearly: SalesData[];
  }>({
    weekly: [
      { name: 'Mon', revenue: 2400 },
      { name: 'Tue', revenue: 1398 },
      { name: 'Wed', revenue: 3800 },
      { name: 'Thu', revenue: 3908 },
      { name: 'Fri', revenue: 4800 },
      { name: 'Sat', revenue: 5800 },
      { name: 'Sun', revenue: 4300 },
    ],
    monthly: [
      { name: 'Jan', revenue: 12000 },
      { name: 'Feb', revenue: 15000 },
      { name: 'Mar', revenue: 18000 },
      { name: 'Apr', revenue: 22000 },
      { name: 'May', revenue: 20000 },
      { name: 'Jun', revenue: 25000 },
      { name: 'Jul', revenue: 23000 },
      { name: 'Aug', revenue: 27000 },
      { name: 'Sep', revenue: 24000 },
      { name: 'Oct', revenue: 26000 },
      { name: 'Nov', revenue: 30000 },
      { name: 'Dec', revenue: 32000 },
    ],
    yearly: [
      { name: '2023', revenue: 250000 },
      { name: '2024', revenue: 300000 },
      { name: '2025', revenue: 350000 },
    ],
  });

  const [lowStockItems] = useState<LowStockItem[]>([
    { id: 1, name: 'Blue Denim Jacket', sku: 'BDJ-001', stock: 3, category: 'Outerwear' },
    { id: 2, name: 'White Cotton T-Shirt', sku: 'WTS-023', stock: 5, category: 'T-Shirts' },
    { id: 3, name: 'Black Slim Jeans', sku: 'BSJ-112', stock: 2, category: 'Pants' },
  ]);

  const [recentOrders] = useState<RecentOrder[]>([
    { id: '#3245', customer: 'John Smith', date: '2025-04-23', status: 'Pending', total: '$128.50' },
    { id: '#3244', customer: 'Emma Johnson', date: '2025-04-22', status: 'Shipped', total: '$235.00' },
    { id: '#3243', customer: 'Michael Brown', date: '2025-04-22', status: 'Delivered', total: '$78.99' },
    { id: '#3242', customer: 'Sophia Williams', date: '2025-04-21', status: 'Processing', total: '$189.75' },
  ]);

  const [recentReviews] = useState<RecentReview[]>([
    { id: 1, product: 'Premium Wool Sweater', customer: 'Alex K.', rating: 5, comment: 'Excellent quality, very warm and comfortable!' },
    { id: 2, product: 'Classic Fitted Shirt', customer: 'Tanya M.', rating: 4, comment: 'Great fit, but the material is a bit thin.' },
  ]);

  return (
    <main className="flex-1 overflow-auto transition-all duration-300 ml-16">
      <div className="container mx-auto px-4 py-8">
        <h1 className="text-3xl font-bold text-gray-800 mb-8">Dashboard</h1>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4 mb-8">
          <StatsCard title="Daily Sales" value={stats.dailySales} icon="ShoppingBag" />
          <StatsCard title="Pending Orders" value={stats.pendingOrders} icon="Package" />
          <StatsCard title="Low Stock Items" value={stats.lowStock} icon="AlertCircle" />
          <StatsCard title="New Customers" value={stats.newCustomers} icon="Users" />
          <StatsCard title="Revenue" value={`$${stats.totalRevenue.toLocaleString()}`} icon="DollarSign" />
        </div>
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          <SalesChart data={salesData} />
          <LowStockItems items={lowStockItems} />
          <RecentOrders orders={recentOrders} />
          <RecentReviews reviews={recentReviews} />
        </div>
      </div>
    </main>
  );
};

export default AdminDashboard;