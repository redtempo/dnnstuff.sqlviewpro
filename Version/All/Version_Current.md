

* Enhancements
    * Added paging support to Template report
    * Added additional cache scheme to data queries - you can now choose from Sliding (the default) or Absolute
        * Sliding cache resets the timeout each time it's hit so if you have a busy site the data might stay in cache a long time
        * Absolute cache expires after a set number of seconds regardless of whether it's been accessed and will reload at that expiry

For additional release history please visit the [documentation](http://docs.dnnstuff.com/pages/sqlviewpro).
