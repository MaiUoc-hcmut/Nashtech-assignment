export interface ParentCategory {
    id: number,
    name: string,
    description: string
}

export interface Product {
    id: number;
    name: string;
    rating: number;
    imageUrl: string;
    price: number;
    updatedAt: Date | '2025-04-28'
}

export interface AddProduct {
  name: string;
  description: string;
  price: string;
  classifications: string;
  image: File[] | null;
  variants: Variant[] | null;
}

export interface Variant {
  id: number;
  colorId: number;
  sizeId: number;
  sku: string;
  imagePreview: string | null;
  price: number;
  stockQuantity: number;
  SKU: string;
  Categories: number[];
}
  
export interface ProductCardProps {
    product: Product;
}
  
export interface StarRatingProps {
    rating: number;
}

export interface Order {
    id: number;
    amount: number;
    status: string;
    address: string;
    createdAt: string;
    customer: string;
}

export interface Review {
    id: number;
    rating: number;
    text: string;
    createdAt: string;
    customer: string
    product: string;
}

export interface Customer {
    id: number;
    name: string;
    email: string;
    phoneNumber: string;
    address: string;
}

export interface Admin {
    id: number;
    name: string;
    email: string;
    phoneNumber: string;
    address: string;
}