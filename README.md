## Test Scenario

### Categories

```
{
  id: 1,
  products: [{id: 1}],
  subcategories: [
    {
      id: 2,
      products: [{id: 2}],
      subcategories: [
        {
          id: 4,
          products: [{id: 4}],
          subcategories: [],
        }
      ],
    },
    {
      id: 3,
      products: [{id: 3}],
      subcategories: [],
    }
  ],
}

```

### Orders

```
[
  {
    id: 1,
    products: [ 
      { id: 1, price: 10 },
      { id: 2, price: 11 },
      { id: 3, price: 12 },
      { id: 4, price: 13 },
    ]
  },
  {
    id: 2,
    products: [ 
      { id: 1, price: 14 },
      { id: 2, price: 15 },
      { id: 3, price: 16 },
      { id: 4, price: 17 },
    ]
  },
  {
    id: 3,
    products: [ 
      { id: 1, price: 18 },
      { id: 2, price: 19 },
      { id: 3, price: 20 },
      { id: 4, price: 21 },
    ]
  },
  {
    id: 4,
    products: [ 
      { id: 1, price: 22 },
      { id: 2, price: 23 },
      { id: 3, price: 24 },
      { id: 4, price: 25 },
    ]
  },
]
```

### Test GetOrderStatistics

Input : 
```
[
  {
    OrderId: 1,
    Orders: [
      Product1,
      Product2,
      Product3,
      Product4,
    ]
  },
  {
    OrderId: 2,
    Orders: [
      Product1,
      Product2,
      Product3,
      Product4,
    ]
  },
  {
    OrderId: 3,
    Orders: [
      Product1,
      Product2,
      Product3,
      Product4,
    ]
  },
  {
    OrderId: 4,
    Orders: [
      Product1,
      Product2,
      Product3,
      Product4,
    ]
  },
]
```

Result:
```
{
  category1:
  {
    Count: 4 + category1.Count + category2.Count + category3.Count = 16,
    Sum: 10 + 14 + 18 + 22 + category2.Sum + category3.Sum = 64 + 140 + 72 = 280
  },
  category2:
  {
    Count: 4 + category4.Count = 8,
    Sum: 11 + 15 + 19 + 23 + category4.Sum = 68 + 76 = 144
  },
  category3:
  {
    Count: 4,
    Sum: 12 + 16 + 20 + 24 = 72
  },
  category4:
  {
    Count: 4,
    Sum: 13 + 17 + 21 + 25 = 76
  },
}
```

### GetProductsOfCategoryAndDescendants

Input:
  category1

Result: 
```
  product1 + category2.products + category3.products = 
  [
    product1,
    product2,
    product3,
    product4
  ]
```

Input:
  category2

Result: 
```
  product2 + category4.products = 
  [
    product2,
    Product4
  ]
```

Input:
  category3

Result: 
```
  [product3]
```
  
Input:
  category4

Result: 
```
  [product4]
```
