// redux/slices/authSlice.ts
import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axiosConfig from '../../config/axios.config';

interface AuthState {
  isAuthenticated: boolean;
  admin: any | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: AuthState = {
  isAuthenticated: false,
  admin: null,
  status: 'idle',
  error: null,
};

// Thunk để xử lý đăng nhập
export const login = createAsyncThunk(
    'auth/login/admin', 
    async(payload: {
      email: string,
      password: string
    }) => {
      const response = await axiosConfig.post(`http://localhost:5113/api/Auth/admin/login`, payload);
      if (response.status !== 200) throw new Error('Failed to login');
      return response.data;
    }
);

export const checkAuthState = createAsyncThunk(
  'auth/checkState',
  async (_, { rejectWithValue }) => {
    try {
      const response = await axiosConfig.get('http://localhost:5113/api/Auth/validate');
      return response.data;
    } catch (error) {
      return rejectWithValue('Authentication failed');
    }
  }
);

export const logout = createAsyncThunk(
  'auth/logout',
  async (_, { rejectWithValue }) => {
    try {
      const response = await axiosConfig.post('http://localhost:5113/api/Auth/logout');
      return response.data;
    } catch (error) {
      return rejectWithValue('Authentication failed');
    }
  }
);

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    reset: (state) => {
      const token = localStorage.getItem('admin_token');
      if (token) {
        state.isAuthenticated = true;
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(login.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.isAuthenticated = true;
        state.admin = action.payload.Admin;
      })
      .addCase(login.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(checkAuthState.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(checkAuthState.fulfilled, (state, action) => {
        state.admin = action.payload;
        state.isAuthenticated = true;
        state.error = null;
        state.status = 'succeeded';
      })
      .addCase(checkAuthState.rejected, (state) => {
        state.admin = null;
        state.status = 'failed';
      })
      .addCase(logout.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(logout.fulfilled, (state) => {
        state.status = 'succeeded';
        state.isAuthenticated = false;
        state.admin = null;
      });
  }
});

export const { reset } = authSlice.actions;
export default authSlice.reducer;

