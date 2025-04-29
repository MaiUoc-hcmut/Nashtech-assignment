export interface parentCategory {
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