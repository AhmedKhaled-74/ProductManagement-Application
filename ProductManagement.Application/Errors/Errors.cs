using ErrorOr;

namespace ProductManagement.Application.Errors
{
    public static class Errors
    {
        public static class ProductErrors
        {
            public static Error ProductNotFound => Error.NotFound(code: "Product.NotFound",
                description: "Product with this Id Notfound"
                ); 

            public static Error FilteredProductsNotFound(int page) => Error.NotFound(code: "Product.NotFound",
                description: $"there is no page number {page} contain products"
                );
            public static Error ProductCustomAttributeNotFound => Error.NotFound(code: "Product.ProductCustomAttributeNotFound",
                description: $"there is no ProductCustomAttribute NotFound for this id"
                );
            public static Error ProductMediaNotFound => Error.NotFound(code: "Product.ProductMediaNotFound",
                description: $"there is no ProductMediaNotFound NotFound for this id"
                );
            public static Error ProductReviewNotFound => Error.NotFound(code: "Product.ProductReviewNotFound",
                description: $"there is no ProductReviewNotFound NotFound for this id"
                );
            public static Error DuplicatedProduct => Error.Conflict(code: "Product.Duplicated",
                    description: "Product with this Name is aleardy exsist"
                );
            public static Error ProductIdRequired => Error.Validation(code: "Product.IdRequired",
                description: "ProductId is Required"
                );
            public static Error ProductObjectRequired(string objName) => Error.Validation(code: "Product.ObjectRequired",
                 description: $"{objName} is Required"
                );

            
        }
        public static class RegionErrors
        {
            public static Error RegionNotFound => Error.NotFound(code: "Region.NotFound",
                description: "Region with this Name Notfound"
                );
            public static Error RegionIdRequired => Error.Validation(code: "Region.IdRequired",
                description: "RegionId is Required"
                );
            public static Error RegionObjectRequired => Error.Validation(code: "Region.ObjectRequired",
                 description: "Object is Required"
                );
            public static Error DuplicatedRegion => Error.Conflict(code: "Region.Duplicated",
                description: "Region with this Name is aleardy exsist"
                );
        }
        public static class CategoryErrors
        {
            public static Error CategoryNotFound => Error.NotFound(code: "Category.NotFound",
                description: "Category with this Id Notfound"
                );
            public static Error CategoryIdRequired => Error.Validation(code: "Category.IdRequired",
                description: "CategoryId is Required"
                );
            public static Error CategoryObjectRequired => Error.Validation(code: "Category.ObjectRequired",
                 description: "Object is Required"
                );
            public static Error DuplicatedCategory => Error.Conflict(code: "Category.Duplicated",
                description: "Category with this Name is aleardy exsist"
                );
        }
        public static class SubCategoryErrors
        {
            public static Error SubCategoryNotFound => Error.NotFound(code: "SubCategory.NotFound",
                description: "SubCategory with this Id Notfound"
                );
            public static Error SubCategoryIdRequired => Error.Validation(code: "SubCategory.IdRequired",
                description: "SubCategoryId is Required"
                );
            public static Error SubCategoryObjectRequired => Error.Validation(code: "SubCategory.ObjectRequired",
                 description: "Object is Required"
                );
            public static Error DuplicatedSubCategory => Error.Conflict(code: "SubCategory.Duplicated",
                description: "SubCategory with this Name is aleardy exsist"
                );
        }
        public static class VendorErrors
        {
            public static Error VendorNotFound => Error.NotFound(code: "Vendor.NotFound",
                description: "Vendor with this Id Notfound"
                );
            public static Error UserIsNotVendor => Error.NotFound(code: "Vendor.UserIsNotVendor",
                description: "User with this Id Is Not Vendor"
                );

            public static Error VendorDoesntHaveBrandAuthorize => Error.Conflict(code: "Vendor.VendorDoesntHaveBrandAuthorize",
                description: "Vendor with this id doesnt have this brand authorize"
                );
        }
        public static class CartErrors
        {
            public static Error CartNotFound => Error.NotFound(code: "Cart.NotFound",
                description: "Cart with this Id Notfound For This User"
                );
            public static Error CartIdRequired => Error.Validation(code: "Cart.IdRequired",
                description: "CartId is Required"
                );
            public static Error CartObjectRequired => Error.Validation(code: "Cart.ObjectRequired",
                 description: "Object is Required"
                );
            public static Error CartProductNotFound => Error.NotFound(code: "Cart.CartProductNotFound",
                description: "Product Not found in Cart"
                );
            public static Error FailedToAddProductToCart => Error.Failure(code: "Cart.FailedToAddProduct",
                description: "Failed to add product to cart"
                );
            public static Error FailedToUpdateCart => Error.Failure(code: "Cart.FailedToUpdateCart",
                description: "Failed to update cart"
                );
        }
        public static class WishListErrors
        {
            public static Error WishListNotFound => Error.NotFound(code: "WishList.NotFound",
                description: "WishList with this Id Notfound For This User"
            );
            public static Error FailedToAddProductToWishList => Error.Failure(code: "WishList.FailedToAddProduct",
                description: "Failed to add product to wish list"
                );
            public static Error FailedToRemoveProductFromWishList => Error.Failure(code: "WishList.FailedToRemoveProduct",
                description: "Failed to remove product from wish list"
                );
            public static Error ProductAlreadyInWishList => Error.Conflict(code: "WishList.ProductAlreadyExists",
                description: "Product is already in wish list"
                );
            public static Error WishListObjectRequired => Error.Validation(code: "WishList.ObjectRequired",
                 description: "Object is Required"
                );
            public static Error ProductNotInWishList => Error.NotFound(code: "WishList.ProductNotInWishList",
                description: "Product is not in wish list"
                );


        }

    }
}
