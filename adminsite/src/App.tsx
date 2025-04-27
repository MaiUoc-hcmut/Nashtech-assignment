// App.tsx
import { JSX, lazy, Suspense, useEffect, useState } from 'react';
import { BrowserRouter, Routes, Route, Navigate, useLocation } from 'react-router-dom';
import { useAppSelector, useAppDispatch } from './redux/hooks/redux';
import { checkAuthState } from './redux/features/auths/authsSlice';
import LoadingSpinner from './components/LoadingSpiner';
import AdminLayout from './layout';

// Import your page components
const Login = lazy(() => import('./pages/Login')); // Import trang Login


// Component để bảo vệ route yêu cầu xác thực
const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  const { isAuthenticated, status } = useAppSelector((state) => state.auth);
  const location = useLocation();

  // If we're still checking auth status, show loading
  if (status === 'loading') {
    return <LoadingSpinner />;
  }

  if (!isAuthenticated) {
    // Chuyển hướng đến trang đăng nhập và lưu lại đường dẫn hiện tại để redirect sau khi đăng nhập
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return children;
};

// App component with authentication checking
const App: React.FC = () => {
  const dispatch = useAppDispatch();
  const [authChecked, setAuthChecked] = useState(false);
  const { status } = useAppSelector(state => state.auth);

  useEffect(() => {
    // Check authentication status when the app loads
    dispatch(checkAuthState())
      .finally(() => {
        setAuthChecked(true);
      });
  }, [dispatch]);

  // Show loading spinner while checking authentication
  if (!authChecked && status === 'loading') {
    return <LoadingSpinner />;
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route 
          path="/login" 
          element={
            <Suspense fallback={<LoadingSpinner />}>
              <Login />
            </Suspense>
          } 
        />
        <Route 
          path="/*" 
          element={
            <ProtectedRoute>
              <AdminLayout />
            </ProtectedRoute>
          } 
        />
      </Routes>
    </BrowserRouter>
  );
};

export default App;