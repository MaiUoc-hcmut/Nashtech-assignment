import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import axiosConfig from '../../config/axios.config';
import { AddProduct } from '../../../types/globalTypes';

// Define types for your state
interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  rating: number;
  updatedAt: Date
}

interface ProductsState {
  totalProducts: number;
  products: Product[];
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

// Initial state
const initialState: ProductsState = {
  totalProducts: 0,
  products: [],
  status: 'idle',
  error: null,
};

// Create async thunk for fetching products
export const fetchProducts = createAsyncThunk(
  'products/fetchProducts',
  async (currentPage: number) => {
    const response = await axiosConfig.get(`http://localhost:5113/api/Product?pageNumber=${currentPage}`);
    if (response.status !== 200) throw new Error('Failed to fetch products');
    return response.data;
  }
);

export const addProduct = createAsyncThunk(
  'products/addProduct',
  async (data: AddProduct) => {
    console.log(data);
    const formData = new FormData();
    formData.append('name', data.name);
    formData.append('description', data.description);
    formData.append('price', data.price);
    formData.append('classifications', data.classifications);

    if (data.variants !== null) {
      const transformedVariants = data.variants.map(({ id, colorId, sizeId, sku, price, stockQuantity, imagePreview, ...rest }) => ({
        ...rest,
        SKU: sku,
        Price: price,
        StockQuantity: stockQuantity,
        Categories: [colorId, sizeId],
      }));

      formData.append('variants', JSON.stringify(transformedVariants));
    }

    if (data.image && data.image[0]) {
      formData.append('image', data.image[0]);
    }

    const response = await axiosConfig.post('http://localhost:5113/api/Product', formData);
    if (response.status !== 200) throw new Error('Failed to add product');
    return response.data;
  }
);

export const deleteProduct = createAsyncThunk(
  'products/deleteProduct',
  async (data: {id: number, password: string}) => {
    const response = await axiosConfig.delete(`http://localhost:5113/api/Product/${data.id}`, {data: data.password });
    if (response.status !== 200) throw new Error('Failed to delete product');
    return data.id;
  }
);

// Create your slice
const productsSlice = createSlice({
  name: 'products',
  initialState,
  reducers: {
    resetProducts: (state) => {
      state.products = [];
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchProducts.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchProducts.fulfilled, (state, action: PayloadAction<{ totalProducts: number; products: Product[] }>) => {
        state.status = 'succeeded';
        state.totalProducts = action.payload.totalProducts;
        state.products = action.payload.products;
        console.log(action.payload);
        state.error = null;
      })
      .addCase(fetchProducts.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      })
      .addCase(deleteProduct.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(deleteProduct.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.error = null;

        const index = state.products.findIndex(
          (product) => product.id === action.payload
        );
        if (index !== -1) {
          state.products.splice(index, 1);
        }
      })
      .addCase(deleteProduct.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Something went wrong';
      });
  },
});

// Export actions and reducer
export const { resetProducts } = productsSlice.actions;
export default productsSlice.reducer;