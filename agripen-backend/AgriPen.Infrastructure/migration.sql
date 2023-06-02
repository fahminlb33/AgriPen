CREATE FUNCTION dbo.HaversineDistance(@Lat1 float, @Lng1 float, @Lat2 float, @Lng2 float)
RETURNS float
AS
BEGIN
    RETURN (
    	2 * 6335 * ASIN(
    		SQRT(
	            POWER(SIN((RADIANS(@Lat2) - RADIANS(@Lat1)) / 2), 2) + 
	            COS(RADIANS(@Lat1)) *
	            COS(RADIANS(@Lat2)) *
	            POWER(SIN((RADIANS(@Lng2) - RADIANS(@Lng1)) / 2), 2)
        	)
        )
    );
END
