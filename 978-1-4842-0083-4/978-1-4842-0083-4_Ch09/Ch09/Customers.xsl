<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="xml" indent="yes"/>
<xsl:template match="/Extract">
    <Customers>
        <xsl:for-each select="Customers/Customer">
        <Customer>
            <Key>
                <xsl:value-of select="@Key"/>
            </Key>
            <FirstName>
                <xsl:value-of select="Name/FirstName"/>
            </FirstName>
            <LastName>
                <xsl:value-of select="Name/LastName"/>
            </LastName>
            <BirthDate>
                <xsl:value-of select="BirthDate"/>
            </BirthDate>
            <Gender>
                <xsl:value-of select="Gender"/>
            </Gender>
            <YearlyIncome>
                <xsl:value-of select="YearlyIncome"/>
            </YearlyIncome>
        </Customer>
        </xsl:for-each>
    </Customers>
</xsl:template>
</xsl:stylesheet>

