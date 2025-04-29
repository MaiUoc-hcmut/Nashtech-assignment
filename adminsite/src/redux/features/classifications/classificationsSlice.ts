import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import axiosConfig from '../../config/axios.config';

// Define types for your state
interface Classification {
  id: number;
  name: string;
  description: string;
}

interface AddClassification {
  name: string;
  description: string;
}

interface ClassificationsState {
  classifications: Classification[];
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

// Initial state
const initialState: ClassificationsState = {
  classifications: [],
  status: 'idle',
  error: null,
};

// Create async thunks for API calls
export const fetchClassifications = createAsyncThunk(
  'classifications/fetchClassifications',
  async () => {
    const response = await axiosConfig.get('http://localhost:5113/api/Classification');
    if (response.status !== 200) throw new Error('Failed to fetch classifications');
    return response.data;
  }
);

export const searchClassification = createAsyncThunk(
  'classifications/searchClassification',
  async (pattern: string) => {
    const response = await axiosConfig.get(`http://localhost:5113/api/Classification/search?pattern=${pattern}`);
    if (response.status !== 200) throw new Error('Failed to fetch classifications');
    return response.data;
  }
)

export const addClassification = createAsyncThunk(
  'classifications/addClassification',
  async (payload: AddClassification) => {
    const response = await axiosConfig.post('http://localhost:5113/api/Classification', payload);
    if (response.status !== 200) throw new Error('Failed to fetch classifications');
    return response.data;
  }
);

export const updateClassification = createAsyncThunk(
  'classification/updateClassification',
  async (payload: Classification) => {
    const { id, ...classificationData } = payload;
    const response = await axiosConfig.put(`http://localhost:5113/api/Classification/${id}`, classificationData);
    if (response.status !== 200) throw new Error('Failed to update classifications');
    return response.data;
  }
);

export const deleteClassification = createAsyncThunk(
  'classification/deleteClassification',
  async (id: number) => {
    const response = await axiosConfig.delete(`http://localhost:5113/api/Classification/${id}`);
    if (response.status !== 200) throw new Error('Failed to delete classifications');
    return id;
  }
)

// Create your slice
const classificationsSlice = createSlice({
  name: 'classifications',
  initialState,
  reducers: {
    // Regular reducers here
    resetClassification: (state) => {
      state.classifications = [];
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchClassifications.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchClassifications.fulfilled, (state, action: PayloadAction<Classification[]>) => {
        state.status = 'succeeded';
        state.classifications = action.payload;
        state.error = null;
        console.log(action.payload);
      })
      .addCase(fetchClassifications.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(addClassification.pending, (state) => {
        state.status = 'loading'
      })
      .addCase(addClassification.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.classifications.push(action.payload);
        state.error = null;
        console.log(state.classifications);
      })
      .addCase(addClassification.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(updateClassification.pending, (state) => {
        state.status = 'loading'
      })
      .addCase(updateClassification.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.error = null;

        const index = state.classifications.findIndex(
          (classification) => classification.id === action.payload.id
        );
        if (index !== -1) {
          state.classifications[index] = action.payload;
        }
      })
      .addCase(updateClassification.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(deleteClassification.pending, (state) => {
        state.status = 'loading'
      })
      .addCase(deleteClassification.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.error = null;

        const index = state.classifications.findIndex(
          (classification) => classification.id === action.payload
        );
        if (index !== -1) {
          state.classifications.splice(index, 1);
        }
      })
      .addCase(deleteClassification.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      });
  },
});

// Export actions and reducer
export const { resetClassification } = classificationsSlice.actions;
export default classificationsSlice.reducer;