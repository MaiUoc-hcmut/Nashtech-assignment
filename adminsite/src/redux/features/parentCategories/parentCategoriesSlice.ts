import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import axiosConfig from '../../config/axios.config';

// Define types for your state
interface ParentCategory {
  id: number;
  name: string;
  description: string;
}

interface AddParentCategory {
  name: string;
  description: string;
}

interface ParentCategoriesState {
  parentCategories: ParentCategory[];
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

// Initial state
const initialState: ParentCategoriesState = {
  parentCategories: [],
  status: 'idle',
  error: null,
};

// Create async thunks for API calls
export const fetchParentCategories = createAsyncThunk(
  'parentCategories/fetchParentCategories',
  async () => {
    const response = await axiosConfig.get('http://localhost:5113/api/ParentCategory');
    if (response.status !== 200) throw new Error('Failed to fetch parent categories');
    return response.data;
  }
);

export const addParentCategory = createAsyncThunk(
  'parentCategories/addParentCategory',
  async(payload: AddParentCategory) => {
    const response = await axiosConfig.post('http://localhost:5113/api/ParentCategory', payload);
    if (response.status !== 200) throw new Error('Failed to add parent category');
    return response.data;
  }
);

export const updateParentCategory = createAsyncThunk(
  'parentCategories/updateParentCategory',
  async(payload: ParentCategory) => {
    const { id, ...parentCategoryData } = payload;
    const response = await axiosConfig.put(`http://localhost:5113/api/ParentCategory/${id}`, parentCategoryData);
    if (response.status !== 200) throw new Error('Failed to edit parent category');
    return response.data;
  }
);

export const deleteParentCategory = createAsyncThunk(
  'parentCategories/deleteParentCategory',
  async(id: number) => {
    const response = await axiosConfig.delete(`http://localhost:5113/api/ParentCategory/${id}`);
    if (response.status !== 200) throw new Error('Failed to delete parent category');
    return id;
  }
);

// Create your slice
const parentCategoriesSlice = createSlice({
  name: 'parentCategories',
  initialState,
  reducers: {
    // Regular reducers here
    resetParentCategories: (state) => {
      state.parentCategories = [];
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchParentCategories.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchParentCategories.fulfilled, (state, action: PayloadAction<ParentCategory[]>) => {
        state.status = 'succeeded';
        state.parentCategories = action.payload;
        state.error = null;
      })
      .addCase(fetchParentCategories.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(addParentCategory.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(addParentCategory.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.parentCategories.push(action.payload);
        state.error = null;
        console.log(action.payload);
      })
      .addCase(addParentCategory.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(updateParentCategory.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(updateParentCategory.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.error = null;

        const index = state.parentCategories.findIndex(
          (parentCategory) => parentCategory.id === action.payload.id
        );
        if (index !== -1) {
          state.parentCategories[index] = action.payload;
        }
      })
      .addCase(updateParentCategory.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(deleteParentCategory.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(deleteParentCategory.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.error = null;

        const index = state.parentCategories.findIndex(
          (parentCategory) => parentCategory.id === action.payload
        );
        if (index !== -1) {
          state.parentCategories.splice(index, 1);
        }
      })
      .addCase(deleteParentCategory.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      });
  },
});

// Export actions and reducer
export const { resetParentCategories } = parentCategoriesSlice.actions;
export default parentCategoriesSlice.reducer;