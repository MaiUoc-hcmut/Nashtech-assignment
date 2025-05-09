import { configureStore } from '@reduxjs/toolkit';
import parentCategoriesReducer from '../features/parentCategories/parentCategoriesSlice';
import categoriesReducer from '../features/categories/categoriesSlice';
import classificationReducer from '../features/classifications/classificationsSlice';
import authReducer from '../features/auths/authsSlice';
import productReducer from '../features/products/productsSlice';
import orderReducer from '../features/orders/ordersSlice';
import reviewReducer from '../features/reviews/reviewsSlice';
import customerReducer from '../features/customers/customersSlice';

export const store = configureStore({
  reducer: {
    categories: categoriesReducer,
    parentCategories: parentCategoriesReducer,
    classifications: classificationReducer,
    auth: authReducer,
    products: productReducer,
    orders: orderReducer,
    reviews: reviewReducer,
    customers: customerReducer
  },
});

// Export types for TypeScript
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;