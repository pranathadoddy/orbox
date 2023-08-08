DELIMITER //

DROP PROCEDURE rpt_usp_GetDailyRevenueChart;

CREATE PROCEDURE `rpt_usp_GetDailyRevenueChart`(
	IN tenantId BIGINT UNSIGNED,
    IN date DATETIME
)
BEGIN

SET @tenantId:= tenantId;
SET @date:= date;
SET @id:=0;

SELECT 
	@id:=@id+1 'Id',
	CAST(DAY(DateList.DateField) AS CHAR(2)) 'Key', 
    IFNULL(SUM(oi.Quantity*oi.UnitPrice), 0) 'Value', 
    CONCAT(IFNULL(t.Currency, ''), ' ', CONVERT(FORMAT(IFNULL(SUM(oi.Quantity*oi.UnitPrice), 0), 0) USING latin1)) 'Display'
FROM
(
    SELECT
        MAKEDATE(YEAR(@date),1) +
        INTERVAL(MONTH(@date)-1) MONTH +
        INTERVAL daynum DAY DateField
    FROM
    (
        SELECT t*10+u daynum
        FROM
            (SELECT 0 t UNION SELECT 1 UNION SELECT 2 UNION SELECT 3) A,
            (SELECT 0 u UNION SELECT 1 UNION SELECT 2 UNION SELECT 3
            UNION SELECT 4 UNION SELECT 5 UNION SELECT 6 UNION SELECT 7
            UNION SELECT 8 UNION SELECT 9) B
        ORDER BY daynum
    ) AA
) DateList
LEFT JOIN com_Order o ON o.TenantId = @tenantId AND DATE_FORMAT(o.Date, '%Y-%m-%d') = DateList.DateField AND o.Status = 'FIN'
LEFT JOIN com_OrderItem oi ON oi.OrderId = o.Id
LEFT JOIN com_Tenant t ON t.Id = o.TenantId
WHERE MONTH(DateList.DateField) = MONTH(@date)
GROUP BY DateList.DateField, t.Currency
ORDER BY DateList.DateField;

END //