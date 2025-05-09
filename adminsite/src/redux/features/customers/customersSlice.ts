import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { Customer } from '../../../types/globalTypes';
import axiosConfig from '../../config/axios.config';

interface CustomersState {
  totalCustomers: number;
  customers: Customer[];
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: CustomersState = {
  totalCustomers: 0,
  customers: [],
  status: 'idle',
  error: null,
};

// Async thunks
export const fetchCustomers = createAsyncThunk(
  'customers/fetchCustomers',
  async (currentPage: number) => {
    try {
      const response = await axiosConfig.get(`http://localhost:5113/api/Customer?pageNumber=${currentPage}`);
      return response.data;
    } catch (error) {
      console.log(error);
      throw error;
    }
  }
);

export const fetchCustomerInfo = createAsyncThunk(
  'customers/fetchCustomerInfo',
  async (id: number) => {
    try {
      const response = await axiosConfig.get(`http://localhost:5113/api/Customer/${id}`);
      return response.data;
    } catch (error) {
      console.log(error);
      throw error;
    }
  }
);

const customersSlice = createSlice({
  name: 'customers',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Fetch customers
      .addCase(fetchCustomers.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(
        fetchCustomers.fulfilled,
        (state, action: PayloadAction<{ totalCustomers: number; customers: Customer[] }>) => {
          state.status = 'succeeded';
          state.totalCustomers = action.payload.totalCustomers;
          state.customers = action.payload.customers;
          state.error = null;
        }
      )
      .addCase(fetchCustomers.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Failed to fetch customers';
      });
  },
});

export default customersSlice.reducer;