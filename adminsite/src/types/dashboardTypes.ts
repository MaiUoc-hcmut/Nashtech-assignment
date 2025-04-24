export interface Stats {
  dailySales: number;
  pendingOrders: number;
  lowStock: number;
  newCustomers: number;
  totalRevenue: number;
}

export interface SalesData {
  name: string;
  revenue: number;
}

export enum TimePeriod {
  Weekly = 'weekly',
  Monthly = 'monthly',
  Yearly = 'yearly'
}

export interface LowStockItem {
  id: number;
  name: string;
  sku: string;
  stock: number;
  category: string;
}

export interface RecentOrder {
  id: string;
  customer: string;
  date: string;
  status: string;
  total: string;
}

export interface RecentReview {
  id: number;
  product: string;
  customer: string;
  rating: number;
  comment: string;
}

export interface MenuItem {
  id: string;
  label: string;
  icon: React.ComponentType<{ size?: number; className?: string }>;
}

export interface ClassificationNCateGoryNParent {
  id: number;
  name: string;
  description: string;
}