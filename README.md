## Test Scenario

### Categories

[category1]
├── product1
├── [category2]
│   ├── product2
│   └── [category4]
│       └── product4
└── [category3]
    └── product3

### Orders

- **Order1**
-- Product1 Price: 10
-- Product2 Price: 11
-- Product3 Price: 12
-- Product4 Price: 13
- **Order2**
-- Product1 Price: 14
-- Product2 Price: 15
-- Product3 Price: 16
-- Product4 Price: 17
- **Order3**
-- Product1 Price: 18
-- Product2 Price: 19
-- Product3 Price: 20
-- Product4 Price: 21
- **Order4**
-- Product1 Price: 22
-- Product2 Price: 23
-- Product3 Price: 24
-- Product4 Price: 25


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
  product1 + category2.products + category3.products = [
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
  product2 + category4.products = [
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
