import React from 'react';
import { Routes, Route, Navigate, useLocation } from 'react-router-dom';
import { useNav } from '../contexts/NavContext';
import Sidebar from '../components/Sidebar';
import { lazy, Suspense } from 'react';
import ProductDetail from '../pages/ProductDetail';

const Dashboard = lazy(() => import('../pages/AdminDashboard'));
const Classifications = lazy(() => import('../pages/ClassificationsManagement'));
const Categories = lazy(() => import('../pages/CategoriesManagement'));
const ParentCategory = lazy(() => import('../pages/ParentCategoriesManagement'));
const Products = lazy(() => import('../pages/ProductsManagement'));
// const Orders = lazy(() => import('../pages/OrdersManagement'));
// const Reviews = lazy(() => import('../pages/ReviewsManagement'));
// const Customers = lazy(() => import('../pages/CustomersManagement'));
// const Analytics = lazy(() => import('../pages/AnalyticsManagement'));
// const Settings = lazy(() => import('../pages/SettingsManagement'));

const AdminLayout: React.FC = () => {
  const { collapsed, setCollapsed } = useNav();
  const location = useLocation();
  
  // Extract the current page from URL path
  const currentPage = location.pathname.split('/').pop() || 'dashboard';
  
  return (
    <div className="flex h-screen bg-black-900">
      <Sidebar 
        collapsed={collapsed} 
        setCollapsed={setCollapsed} 
        currentPage={currentPage}
      />
      
      <div className="flex-1 flex flex-col overflow-hidden">
        <main className="flex-1 overflow-x-hidden overflow-y-auto bg-white">
          <Suspense fallback={<div className="flex justify-center items-center h-full">Loading...</div>}>
            <Routes>
              <Route path="dashboard" element={<Dashboard />} />
              <Route path="classifications" element={<Classifications />} />
              <Route path="parentCategories" element={<ParentCategory />} />
              <Route path="categories" element={<Categories />} />
              <Route path="products" element={<Products />} >
                <Route path="detail/:id" element={<ProductDetail />} />
              </Route>
              {/* <Route path="orders" element={<Orders />} />
              <Route path="reviews" element={<Reviews />} />
              <Route path="customers" element={<Customers />} />
              <Route path="analytics" element={<Analytics />} />
              <Route path="settings" element={<Settings />} /> */}
              {/* Redirect to dashboard if no route matches */}
              <Route path="*" element={<Navigate to="dashboard" replace />} />
            </Routes>
          </Suspense>
        </main>
      </div>
    </div>
  );
};

export default AdminLayout;