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
    createdAt: string;
    customer: string;
}

export interface Review {
    id: number;
    rating: number;
    text: string;
    createdAt: string;
    customer: {
        id: number;
        name: string;
    };
    product: Product;
}