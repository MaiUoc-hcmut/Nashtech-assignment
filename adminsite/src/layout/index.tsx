import { useNav } from '../contexts/NavContext';
import Sidebar from '../components/Sidebar';

import { lazy, Suspense } from 'react';


const Dashboard = lazy(() => import('../pages/AdminDashboard'));
const Classifications = lazy(() => import('../pages/ClassificationsManagement'));
const Categories = lazy(() => import('../pages/CategoriesManagement'));


const AdminLayout: React.FC = () => {
    const { currentPage, collapsed, setCollapsed, setCurrentPage } = useNav();
  
    const renderContent = () => {
      return (
        <Suspense fallback={
          <div className="flex items-center justify-center h-full">
            <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
          </div>
        }>
          {(() => {
            switch (currentPage) {
              case 'dashboard': 
                return <Dashboard />;
              case 'classifications': 
                return <Classifications />;
              case 'categories': 
                return <Categories />;
              default: 
                return <Dashboard />;
            }
          })()}
        </Suspense>
      );
    };
  
    return (
      <div className="flex h-screen w-full overflow-hidden">
        <Sidebar 
          collapsed={collapsed} 
          setCollapsed={setCollapsed}
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
        />
        <div className="flex-1 bg-gray-100 p-6 overflow-auto">
          {renderContent()}
        </div>
      </div>
    );
};


export default AdminLayout;