import React, { lazy, Suspense, useEffect, useState } from 'react';
import { BrowserRouter, Routes, Route, Navigate, useLocation } from 'react-router-dom';
import { useAppSelector, useAppDispatch } from './redux/hooks/redux';
import { checkAuthState } from './redux/features/auths/authsSlice';
import LoadingSpinner from './components/LoadingSpiner';

// Import your page components
const Login = lazy(() => import('./pages/Login')); 
const AdminLayout = lazy(() => import('./layout'));

// Component to protect routes requiring authentication
const ProtectedRoute = ({ children }: { children: React.ReactElement }) => {
  const { isAuthenticated, status } = useAppSelector((state) => state.auth);
  const location = useLocation();

  // If we're still checking auth status, show loading
  if (status === 'loading') {
    return <LoadingSpinner />;
  }

  if (!isAuthenticated) {
    // Redirect to login page and save current path for redirect after login
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
      <Suspense fallback={<LoadingSpinner />}>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/admin/*" element={
            <ProtectedRoute>
              <AdminLayout />
            </ProtectedRoute>
          } />
          {/* Redirect root to admin dashboard */}
          <Route path="/" element={<Navigate to="/admin/dashboard" replace />} />
          {/* Catch all route - redirect to dashboard */}
          <Route path="*" element={<Navigate to="/admin/dashboard" replace />} />
        </Routes>
      </Suspense>
    </BrowserRouter>
  );
};

export default App;