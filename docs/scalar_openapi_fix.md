# Scalar OpenAPI Documentation Fix

## Problem
The application was throwing an error when trying to load the Scalar API documentation:
```
System.InvalidOperationException: Unsupported HTTP method:
at ApiDescriptionExtensions.GetOperationType(ApiDescription apiDescription)
```

## Root Causes
The OpenAPI generator was encountering endpoints with invalid or missing HTTP method configurations that it couldn't process:

1. **GET request with body parameter** - `ProductAdminController.GetUnapprovedProducts` had `[FromBody]` on a GET endpoint
2. **DELETE requests with body parameters** - Multiple DELETE endpoints in `ProductAdminController` used `[FromBody]` instead of `[FromQuery]`
3. **Endpoint without HTTP method** - `ErrorController.HandleError` had no HTTP method attribute

## Fixes Applied

### 1. Program.cs
- Added `.ExcludeFromDescription()` to `MapOpenApi()` to prevent the OpenAPI endpoint itself from being included in the documentation

### 2. ProductAdminController.cs
Changed the following endpoints from `[FromBody]` to `[FromQuery]`:
- `GetUnapprovedProducts` - Line 124 (GET request)
- `ApproveProduct` - Line 34 (POST request, changed for consistency)
- `DeleteProduct` - Line 52 (DELETE request)
- `DeleteCustomAttribute` - Line 70 (DELETE request)
- `DeleteProductMedia` - Line 88 (DELETE request)
- `DeleteProductReview` - Line 106 (DELETE request)

### 3. ErrorController.cs
- Added `[ApiExplorerSettings(IgnoreApi = true)]` to exclude the error handler endpoint from OpenAPI documentation

## Why These Changes Were Necessary

### HTTP GET with Body
HTTP GET requests should not have request bodies according to REST best practices. While technically allowed by HTTP spec, many tools and frameworks don't support it.

### HTTP DELETE with Body
While HTTP DELETE can technically have a body, it's not well-supported by:
- OpenAPI/Swagger specifications
- Many HTTP clients and proxies
- Browser fetch API in some cases

Best practice is to use query parameters or route parameters for DELETE requests.

### Error Handler Endpoint
The error handler is an internal endpoint used by the exception handling middleware. It doesn't need to be documented in the API specification and was causing issues because it had no explicit HTTP method attribute.

## Testing
After applying these fixes, the Scalar documentation should load successfully at `/scalar/v1` endpoint.

## API Client Updates Required
If you have existing API clients calling these endpoints, you'll need to update them to pass the `userId` parameter as a query parameter instead of in the request body:

**Before:**
```http
DELETE /api/v1/Products/{productId}/admin
Content-Type: application/json

"user-id-guid-here"
```

**After:**
```http
DELETE /api/v1/Products/{productId}/admin?userId=user-id-guid-here
```
