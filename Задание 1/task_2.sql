SELECT ProdName, Sellers.Surname, Sellers.Name, (ProdSum / sum(Sales.Quantity) * 100) AS PercentSales		
FROM
(SELECT	Products.ID AS ProdID, Products.Name AS ProdName, (sum(Sales.Quantity)) AS ProdSum, Sellers.ID AS SelID
    FROM	Sales, Products, Sellers
    WHERE Sales.IDProd = Products.ID AND Sellers.ID = Sales.IDSel AND (Sales.Date BETWEEN '01.10.2013' AND '07.10.2013')
    GROUP BY Sellers.ID, Products.ID, Products.Name, Sales.Quantity) as A, Sales, Sellers, Arrivals
WHERE Sales.IDProd = A.ProdID AND A.SelID = Sellers.ID AND A.ProdID = Arrivals.IDProd AND (Arrivals.Date BETWEEN '07.09.2013' AND '07.10.2013')
GROUP BY ProdID, ProdName, Sellers.Surname, Sellers.Name, ProdSum
ORDER BY ProdName, Sellers.Surname, Sellers.Name
