<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    
<xsl:template match="bookstore">
		<html>
			<body>
				<h1>Bookstore</h1>
				<table border="1">
					<tr align="left">
						<th>Title</th>
						<th>Author</th>
						<th>Category</th>
						<th>Year</th>
						<th>Price</th>
					</tr>
					<xsl:for-each select="child::*">
						<tr>
							
							<td>
								<xsl:value-of select="title"/>
							</td>

							<td>
								<xsl:apply-templates select="author" />
							</td>
							
							<td>
							     <xsl:value-of select='@category'/>
						    </td>
							
							<td>
								<xsl:value-of select="year"/>
							</td>
							<td>
								<xsl:value-of select="price"/>
							</td>
						
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
	
		
	<xsl:template match="author">
							
		<xsl:copy-of select="."/>
		
		<xsl:element name="author">;  </xsl:element>
						
	</xsl:template>

</xsl:stylesheet>
