SELECT slr.Surname, slr.Name, SUM(sl.Quantity) as "Количество продаж"
	FROM Sales sl JOIN Sellers slr on slr.ID == sl.IDSel
	WHERE DATE(sl.Date) BETWEEN '2013-10-01' AND '2013-10-07'
	ORDER BY slr.Surname, slr.Name
