unit uVersion;
// TODO [X] Debug the Pascal translation
// TODO [ ] Implement the Java translation
// ENDTODO
// TODO PARAM WinLocation=494;347;900;350
interface
type
  TVersion = class
    private
      class var FVersion : string;
      class var FVersionDebug : integer;
    public
      class function get : string;
      class function getDebug : string;
      class function getAppName : string;
  end;

const
  AppName : string = 'APS-Builder ';

implementation
uses Sysutils;
class function TVersion.get : string;
begin
  result := TVersion.FVersion;
end;

class function TVersion.getDebug : string;
begin
  result := get + '.' + inttostr(TVersion.FversionDebug);
end;

class function TVersion.getAppName: string;
begin
  result := AppName;
end;

initialization
  TVersion.FVersion := '0.99'; TVersion.FVersionDebug := 1253;
// 0.99.0.1253  Compiled by [Iovine] 14.03.2018 13:51:47
// 0.99.0.1252  Compiled by [Iovine] 14.03.2018 13:51:43
// 0.99.0.1251  Compiled by [Iovine] 14.03.2018 13:51:39
// 0.99.0.1250  Compiled by [Iovine] 14.03.2018 13:51:34
// - When pressing ESC, writes
// 0.99.0.1249  Compiled by [Iovine] 14.03.2018 10:26:25
// Seems OK
// 0.99.0.1248  Compiled by [Iovine] 14.03.2018 09:45:19
// 0.99.0.1247  Compiled by [Iovine] 14.03.2018 09:44:26
// 0.99.0.1246  Compiled by [Iovine] 14.03.2018 09:29:02
// 0.99.0.1245  Compiled by [Iovine] 14.03.2018 09:28:09
// - Added support for User (ISSUE#2)
// 0.99.0.1244  Compiled 15.06.2017 14:48:58
// 0.99.0.1243  Compiled 15.06.2017 14:29:14
// Done : Debug the Pascal translation
// 0.99.0.1242  Compiled 15.06.2017 14:25:00
// Done : Debug the Pascal translation
// 0.99.0.1241  Compiled 15.06.2017 14:22:32
// Done : Debug the Pascal translation
// 0.99.0.1240  Compiled 15.06.2017 14:20:12
// 0.99.0.1239  Compiled 12.06.2017 16:12:03
// Support for the PASCAL command line parameter
// 0.99.0.1238  Compiled 12.06.2017 15:30:02
// Save correctly the file version on file
// One more test
// Save comments and new data in the version file
//  TVersion.FVersion := '0.99';  TVersion.FVersionDebug := 1234;

end.
