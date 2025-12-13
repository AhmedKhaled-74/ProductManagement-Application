# Product Management API Endpoints - Testing Guide

## 📋 Overview

This document provides all API endpoints for testing the newly implemented features.

**Base URL:** `http://localhost:{port}/api/v1`

---

## 🎯 New Controllers Created

1. ✅ **ReviewController** - Review reactions (like/dislike)
2. ✅ **ProductAdminController** - Role-based access control
3. ✅ **ProductEnhancementController** - Product tags & related products
4. ✅ **CartController** (Updated) - Cart attribute updates & search
5. ✅ **WishListController** (Updated) - Move to cart & clear

---

## 1️⃣ Review Reactions API

### Add or Update Reaction
**POST** `/api/v1/Reviews/{userId}/reactions`

**Request Body:**
```json
{
  "reviewId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "isLike": true
}
```

**Response:**
```json
{
  "message": "Reaction added/updated successfully",
  "reaction": {
    "reviewReactionId": "guid",
    "reviewId": "guid",
    "userId": "guid",
    "isLike": true,
    "createdAt": "2025-12-10T05:00:00Z"
  }
}
```

---

### Remove Reaction
**DELETE** `/api/v1/Reviews/{userId}/reactions/{reviewId}`

**Response:**
```json
{
  "message": "Reaction removed successfully"
}
```

---

### Get Reaction Counts
**GET** `/api/v1/Reviews/{reviewId}/reactions/count`

**Response:**
```json
{
  "reviewId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "likesCount": 45,
  "dislikesCount": 3
}
```

---

### Get User's Reaction
**GET** `/api/v1/Reviews/{reviewId}/reactions/user/{userId}`

**Response:**
```json
{
  "reviewId": "guid",
  "userId": "guid",
  "hasReaction": true,
  "isLike": true
}
```

---

### Get Review with Reactions
**GET** `/api/v1/Reviews/{reviewId}/with-reactions?userId={userId}`

**Response:**
```json
{
  "reviewId": "guid",
  "likesCount": 45,
  "dislikesCount": 3,
  "userReaction": true
}
```

---

## 2️⃣ Product Administrator API

### Assign Administrator Role
**POST** `/api/v1/ProductAdmin/assign?assignedBy={userId}`

**Request Body:**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "role": "Administrator"
}
```

**Valid Roles:**
- `"Administrator"` - Full access
- `"Assistant"` - Limited access

**Response:**
```json
{
  "message": "Administrator assigned successfully",
  "admin": {
    "productAdministratorId": "guid",
    "userId": "guid",
    "role": "Administrator",
    "assignedAt": "2025-12-10T05:00:00Z",
    "assignedBy": "guid"
  }
}
```

---

### Remove Administrator Role
**DELETE** `/api/v1/ProductAdmin/remove/{userId}`

**Response:**
```json
{
  "message": "Administrator role removed successfully"
}
```

---

### Get Administrator Info
**GET** `/api/v1/ProductAdmin/{userId}`

**Response:**
```json
{
  "productAdministratorId": "guid",
  "userId": "guid",
  "role": "Administrator",
  "assignedAt": "2025-12-10T05:00:00Z",
  "assignedBy": "guid"
}
```

---

### Check if User is Administrator
**GET** `/api/v1/ProductAdmin/{userId}/is-administrator`

**Response:**
```json
{
  "userId": "guid",
  "isAdministrator": true
}
```

---

### Check if User is Assistant
**GET** `/api/v1/ProductAdmin/{userId}/is-assistant`

**Response:**
```json
{
  "userId": "guid",
  "isAssistant": false
}
```

---

### Check User Permission
**GET** `/api/v1/ProductAdmin/{userId}/has-permission/{permission}`

**Valid Permissions:**
- `OverrideSubmission`
- `ManagePricing`
- `ManageCategories`
- `ConfigureAttributes`

**Response:**
```json
{
  "userId": "guid",
  "permission": "ManageCategories",
  "hasPermission": true
}
```

---

## 3️⃣ Product Enhancement API

### Get Product Tags
**GET** `/api/v1/Products/{productId}/tags`

**Response:**
```json
{
  "productId": "guid",
  "tags": ["electronics", "smartphone", "5g", "android"]
}
```

---

### Add Product Tag
**POST** `/api/v1/Products/{productId}/tags`

**Request Body:**
```json
"smartphone"
```

**Response:**
```json
{
  "message": "Tag added successfully",
  "productId": "guid",
  "tagName": "smartphone"
}
```

---

### Remove Product Tag
**DELETE** `/api/v1/Products/{productId}/tags/{tagName}`

**Response:**
```json
{
  "message": "Tag removed successfully",
  "productId": "guid",
  "tagName": "smartphone"
}
```

---

### Get Related Products
**GET** `/api/v1/Products/{productId}/related?limit=10`

**Response:**
```json
{
  "productId": "guid",
  "relatedProducts": [
    {
      "productId": "guid",
      "productName": "Samsung Galaxy S24",
      "productPrice": 999.99,
      "productImage": "https://example.com/image.jpg",
      "rating": 4.5,
      "discount": 10.00
    }
  ]
}
```

---

### Set Related Products
**PUT** `/api/v1/Products/{productId}/related`

**Request Body:**
```json
[
  "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "4fb96f75-6828-5673-c4gd-3d074g77bgb7",
  "5gc07g86-7939-6784-d5he-4e185h88chc8"
]
```

**Response:**
```json
{
  "message": "Related products set successfully",
  "productId": "guid",
  "count": 3
}
```

---

## 4️⃣ Cart Enhancements API

### Update Cart Product Attributes
**PUT** `/api/v1/cart/{cartId}/products/{productId}/attributes`

**Request Body:**
```json
{
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customAttributes": {
    "color": "red",
    "size": "large",
    "material": "cotton"
  }
}
```

**Response:**
```json
{
  "message": "Update cart attributes functionality - implement in service layer",
  "cartId": "guid",
  "productId": "guid",
  "customAttributes": {
    "color": "red",
    "size": "large",
    "material": "cotton"
  }
}
```

**Note:** Service layer implementation needed for full functionality.

---

### Search Cart Products
**GET** `/api/v1/cart/{cartId}/search?keyword=shirt`

**Response:**
```json
{
  "message": "Search cart functionality - implement in service layer",
  "cartId": "guid",
  "keyword": "shirt"
}
```

**Note:** Service layer implementation needed for full functionality.

---

## 5️⃣ Wish List Enhancements API

### Move to Cart
**POST** `/api/v1/wishlist/{wishListId}/move-to-cart`

**Request Body:**
```json
{
  "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "quantity": 2
}
```

**Response:**
```json
{
  "message": "Move to cart functionality - implement in service layer",
  "wishListId": "guid",
  "productId": "guid",
  "quantity": 2
}
```

**Note:** Service layer implementation needed for full functionality.

---

### Clear Wish List
**DELETE** `/api/v1/wishlist/{wishListId}/clear`

**Response:**
```json
{
  "message": "Clear wishlist functionality - implement in service layer",
  "wishListId": "guid"
}
```

**Note:** Service layer implementation needed for full functionality.

---

## 🧪 Testing Workflow

### 1. Review Reactions Testing

```bash
# Step 1: Add a like to a review
POST /api/v1/Reviews/{userId}/reactions
Body: { "reviewId": "review-guid", "isLike": true }

# Step 2: Get reaction counts
GET /api/v1/Reviews/{reviewId}/reactions/count

# Step 3: Change to dislike
POST /api/v1/Reviews/{userId}/reactions
Body: { "reviewId": "review-guid", "isLike": false }

# Step 4: Remove reaction
DELETE /api/v1/Reviews/{userId}/reactions/{reviewId}
```

---

### 2. Product Admin Testing

```bash
# Step 1: Assign administrator role
POST /api/v1/ProductAdmin/assign?assignedBy={admin-user-id}
Body: { "userId": "new-admin-guid", "role": "Administrator" }

# Step 2: Check if user is administrator
GET /api/v1/ProductAdmin/{userId}/is-administrator

# Step 3: Check specific permission
GET /api/v1/ProductAdmin/{userId}/has-permission/ManageCategories

# Step 4: Assign assistant role
POST /api/v1/ProductAdmin/assign?assignedBy={admin-user-id}
Body: { "userId": "assistant-guid", "role": "Assistant" }

# Step 5: Verify assistant has limited permissions
GET /api/v1/ProductAdmin/{assistant-id}/has-permission/ManageCategories
# Should return false

# Step 6: Remove administrator
DELETE /api/v1/ProductAdmin/remove/{userId}
```

---

### 3. Product Enhancement Testing

```bash
# Step 1: Add tags to product
POST /api/v1/Products/{productId}/tags
Body: "electronics"

POST /api/v1/Products/{productId}/tags
Body: "smartphone"

# Step 2: Get all tags
GET /api/v1/Products/{productId}/tags

# Step 3: Set related products
PUT /api/v1/Products/{productId}/related
Body: ["related-product-1-guid", "related-product-2-guid"]

# Step 4: Get related products
GET /api/v1/Products/{productId}/related?limit=10

# Step 5: Remove a tag
DELETE /api/v1/Products/{productId}/tags/electronics
```

---

### 4. Cart Enhancement Testing

```bash
# Step 1: Update product attributes in cart
PUT /api/v1/cart/{cartId}/products/{productId}/attributes
Body: {
  "productId": "product-guid",
  "customAttributes": { "color": "blue", "size": "medium" }
}

# Step 2: Search within cart
GET /api/v1/cart/{cartId}/search?keyword=shirt
```

---

### 5. Wish List Enhancement Testing

```bash
# Step 1: Move item from wishlist to cart
POST /api/v1/wishlist/{wishListId}/move-to-cart
Body: { "productId": "product-guid", "quantity": 1 }

# Step 2: Clear entire wishlist
DELETE /api/v1/wishlist/{wishListId}/clear
```

---

## 📝 Important Notes

### Service Layer Implementation Required

The following endpoints have placeholder responses and need service layer implementation:

1. **Cart Enhancements:**
   - `UpdateCartProductAttributes`
   - `SearchCart`

2. **Wish List Enhancements:**
   - `MoveToCart`
   - `ClearWishList`

These endpoints are defined in the controllers but need corresponding service methods to be fully functional.

---

## 🔐 Permission Matrix

| Permission | Administrator | Assistant |
|------------|---------------|-----------|
| OverrideSubmission | ✅ | ✅ |
| ManagePricing | ✅ | ✅ |
| ManageCategories | ✅ | ❌ |
| ConfigureAttributes | ✅ | ❌ |

---

## 📊 Summary

**Total New Endpoints:** 23

| Controller | New Endpoints | Updated Endpoints |
|------------|---------------|-------------------|
| ReviewController | 5 | 0 |
| ProductAdminController | 6 | 0 |
| ProductEnhancementController | 5 | 0 |
| CartController | 2 | 0 |
| WishListController | 2 | 0 |

**All endpoints are ready for testing after:**
1. ✅ Building the solution
2. ✅ Running database migration
3. ✅ Starting the API

**Next Steps:**
1. Implement service layer methods for cart and wishlist enhancements
2. Add authorization middleware for ProductAdmin endpoints
3. Test all endpoints with Postman or Swagger
