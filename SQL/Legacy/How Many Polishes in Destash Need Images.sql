Select b.Name as BrandName,p.Name as PolishName,ppi.id-- ppi.*, pdi.*
From Polishes p 
LEFT JOIN Polishes_Images ppi
	on p.ID = ppi.PolishID
	--AND ppi.ID IS NULL
INNER JOIN Polishes_DestashInfo pdi
	ON p.ID = pdi.PolishID
	AND (SaleStatus != 'S' OR SaleStatus IS NULL)
INNER JOIN Brands b
	ON p.BrandID = b.ID
Where ppi.ID IS NULL
Order by b.Name