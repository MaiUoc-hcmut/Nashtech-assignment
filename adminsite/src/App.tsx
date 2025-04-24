import { useState, useEffect, lazy, Suspense } from 'react';
import Sidebar from './components/Sidebar';

// Import your page components (create these as needed)
const Dashboard = lazy(() => import('./pages/AdminDashboard'));
const Classifications = lazy(() => import('./pages/ClassificationsManagement'));
const Categories = lazy(() => import('./pages/CategoriesManagement'));
// import Products from './pages/Products';
// import Orders from './pages/Orders';
// import Reviews from './pages/Reviews';
// import Customers from './pages/Customers';
// import Analytics from './pages/Analytics';
// import Settings from './pages/Settings';

const App: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const [currentPage, setCurrentPage] = useState<string>(() => {
    return localStorage.getItem('currentPage') || 'dashboard';
  });

  useEffect(() => {
    localStorage.setItem('currentPage', currentPage);
  }, [currentPage]);

  // Render the appropriate component based on currentPage
  const renderContent = () => {
    // Wrap in Suspense to handle loading state
    return (
      <Suspense fallback={<div>Loading page...</div>}>
        {(() => {
          switch (currentPage) {
            case 'dashboard':
              return <Dashboard />;
            case 'classifications':
              return <Classifications />;
            case 'categories':
              return <Categories />;
            // Other cases...
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

export default App;