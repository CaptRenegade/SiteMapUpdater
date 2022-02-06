# SiteMapUpdater
Windows System Service for updating web site sitemap <lastmod> timestamps automatically every 24 hours.

This is a utility I created because I couldn't find something like it on the internet.
The Site Map Updater is used to update the <lastmod> time stamp on a web sites sitemap.xml file
to show the site has recently been updated and is current.  Most web sites are not updated daily however
search engines like to see that they are when nightly indexing occurs.  The most up-to-date web sites
will be prioritized ahead of others as their content is shown to be the most recent as denoted by the
<lastmod> timestamp.

I have included a sample_sitemap.xml file to display the formatting in which you sitemap file should be.
The Site Map Updater works by reading the sitemap.xml file line by line and when it reaches the <url> blocks,
creates a table for which it can isolate the various data fields (specifically the <lastmod> timestamp field)
and change it to a properly formatted current time then write to the new sitemap.xml file.  This is all done
by creating a new temp file, writing everything to that temp file, then deleting the old sitemap.xml file,
and finally renaming to the proper sitemap.xml.

This system service requires a supplemental configuration utility called SMUConfigure which tells the system
service where your sitemap.xml files are located.  This configuration utility allows for multiple sitemaps
from different sites to be updated concurrently as each site has its own sitemap.xml file from which the program
reads independently.

I hope this is useful to someone.  Thank you for visiting! :)
