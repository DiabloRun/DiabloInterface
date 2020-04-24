using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Zutatensuppe.D2Reader.Models;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.DiabloInterface.Business.Services
{
    public class HttpServerService
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        private int port = 8080;

        D2Reader.DataReadEventArgs eventargs;

        public HttpServerService(IGameService gameService)
        {
            gameService.DataRead += GameService_DataRead;
            try
            {
                Thread th = new Thread(new ThreadStart(StartListen));
                th.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("An Exception Occurred while Listening :" + e.ToString());
            }
        }

        private void GameService_DataRead(object sender, D2Reader.DataReadEventArgs e)
        {
            this.eventargs = e;
        }

        private string def(string[] arr)
        {
            return "";
        }

        private string max(string str)
        {
            var expr = str.Substring(1, str.Length - 2);
            var split = expr.Split('|');
            if (split.Length == 2) return $"<div class=\"max\">{split[1]}</div>";
            return "";
        }

        private string parse(string str)
        {
            var expr = str.Substring(1, str.Length - 2);
            var split = expr.Split('|');

            string wrap(string value) => $"<span class=\"di-value di-value-{split[0]}\">{value}</span>";

            if (eventargs == null)
            {
                switch (split[0])
                {
                    case "name":
                    case "sc_hc":
                    case "sc":
                    case "hc":
                    case "exp_classic":
                    case "exp":
                    case "classic":
                    case "px":
                    case "level":
                    case "deaths":
                    case "runs":
                    case "atd":
                    case "emg":
                    case "gold":
                    case "mf":
                    case "str":
                    case "ene":
                    case "vit":
                    case "dex":
                    case "frw":
                    case "fhr":
                    case "fcr":
                    case "ias":
                    case "fire":
                    case "cold":
                    case "light":
                    case "poison":
                    case "norm_complete":
                    case "nm_complete":
                    case "hell_complete": return wrap(def(split));
                    default: return str;
                }
            }

            switch (split[0])
            {
                case "name": return wrap(eventargs.Character.Name);
                case "sc_hc": return wrap(eventargs.Character.IsHardcore ? "HARDCORE" : "SOFTCORE");
                case "sc": return wrap(!eventargs.Character.IsHardcore + "");
                case "hc": return wrap(eventargs.Character.IsHardcore + "");
                case "exp_classic": return wrap(eventargs.Character.IsExpansion ? "EXPANSION" : "CLASSIC");
                case "exp": return wrap(eventargs.Character.IsExpansion + "");
                case "classic": return wrap(!eventargs.Character.IsExpansion + "");
                case "px": return wrap(eventargs.CurrentPlayersX + "");
                case "level": return wrap(eventargs.Character.Level + "");
                case "deaths": return wrap(eventargs.Character.Deaths + "");
                case "runs": return wrap(eventargs.GameCounter + "");
                case "atd": return wrap(eventargs.Character.AttackerSelfDamage + "");
                case "emg": return wrap(eventargs.Character.MonsterGold + "");
                case "gold": return wrap(eventargs.Character.Gold + "");
                case "mf": return wrap(eventargs.Character.MagicFind + "");
                case "str": return wrap(eventargs.Character.Strength + "");
                case "ene": return wrap(eventargs.Character.Energy + "");
                case "vit": return wrap(eventargs.Character.Vitality + "");
                case "dex": return wrap(eventargs.Character.Dexterity + "");
                case "frw": return wrap(eventargs.Character.FasterRunWalk + "");
                case "fhr": return wrap(eventargs.Character.FasterHitRecovery + "");
                case "fcr": return wrap(eventargs.Character.FasterCastRate + "");
                case "ias": return wrap(eventargs.Character.IncreasedAttackSpeed + "");
                case "fire": return wrap(eventargs.Character.FireResist + "");
                case "cold": return wrap(eventargs.Character.ColdResist + "");
                case "light": return wrap(eventargs.Character.LightningResist + "");
                case "poison": return wrap(eventargs.Character.PoisonResist + "");
                case "norm_complete": return wrap($@"{eventargs.Quests.ProgressByDifficulty(GameDifficulty.Normal) * 100:0}%");
                case "nm_complete": return wrap($@"{eventargs.Quests.ProgressByDifficulty(GameDifficulty.Nightmare) * 100:0}%");
                case "hell_complete": return wrap($@"{eventargs.Quests.ProgressByDifficulty(GameDifficulty.Hell) * 100:0}%");
                default: return str;
            }
        }

        public void StartListen()
        {
            // Create a listener.
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://+:{port}/");
            listener.Start();
            Console.WriteLine("Listening...");

            string defaultStyle = @"
html { background: black; color: #efefef; font-family: monospace; }
table { margin: 0; padding: 0; border-collapse: collapse; }
td { margin: 0; padding: 0; }
.di-value { display: inline-block; }
.di-value-name { min-width: 15ch; }
.di-value-hc_sc { min-width: 8ch; }
.di-value-exp_classic { min-width: 9ch; }
.di-value-level { min-width: 2ch; }
.di-value-deaths { min-width: 2ch; }
.di-value-runs { min-width: 3ch; }
.di-value-gold { min-width: 7ch; }
.di-value-mf { min-width: 3ch; }
.di-value-emg { min-width: 3ch; }
.di-value-atd { min-width: 3ch; }
.di-value-str { min-width: 3ch; }
.di-value-dex { min-width: 3ch; }
.di-value-vit { min-width: 3ch; }
.di-value-ene { min-width: 3ch; }
.di-value-frw { min-width: 3ch; }
.di-value-fhr { min-width: 3ch; }
.di-value-fcr { min-width: 3ch; }
.di-value-ias { min-width: 3ch; }
.di-value-fire { min-width: 3ch; }
.di-value-cold { min-width: 3ch; }
.di-value-light { min-width: 3ch; }
.di-value-poison { min-width: 3ch; }
.di-value-norm_complete { min-width: 4ch; }
.di-value-nm_complete { min-width: 4ch; }
.di-value-hell_complete { min-width: 4ch; }
";
            string horizontalStyle = @"
.label-value-table td:nth-child(2) { text-align: right; padding-right: 0.5em; }
.label-value-table td:nth-child(2) .di-value { text-align: right; }
";
            string horizontalBody = @"
<table>
    <tr><td colspan=3>{name}</td></tr>
    <tr><td colspan=3>{sc_hc} {exp_classic} /players {px}</td></tr>
    <tr><td colspan=3>LVL: {level} DEATHS: {deaths} RUNS: {runs}</td></tr>
    <tr><td colspan=3>ATD: {atd} EMG: {emg}</td></tr>
    <tr><td colspan=3>GOLD: {gold} MF: {mf}</td></tr>
    <tr><td>
        <table class=""label-value-table"">
                <tr><td>STR:</td><td>{str}</td></tr>
                <tr><td>DEX:</td><td>{dex}</td></tr>
                <tr><td>VIT:</td><td>{vit}</td></tr>
                <tr><td>ENE:</td><td>{ene}</td></tr>
        </table>
    </td><td>
        <table class=""label-value-table"">
                <tr><td>FRW:</td><td>{frw}</td></tr>
                <tr><td>FHR:</td><td>{fhr}</td></tr>
                <tr><td>FCR:</td><td>{fcr}</td></tr>
                <tr><td>IAS:</td><td>{ias}</td></tr>
        </table>
    </td><td>
        <table class=""label-value-table"">
                <tr><td>FIRE:</td><td>{fire}</td> </tr>
                <tr><td>COLD:</td><td>{cold}</td> </tr>
                <tr><td>LIGH:</td><td>{light}</td> </tr>
                <tr><td>POIS:</td><td>{poison}</td> </tr>
        </table>
    </td></tr>
    <tr><td colspan=3>NO: {norm_complete} NM: {nm_complete} HE: {hell_complete}</td></tr>
</table>";

            string verticalStyle = @"
.label-value-table td:nth-child(2) { text-align: right; padding-right: 0.5em; }
";
            string verticalBody = @"
<table class=""label-value-table"">
    <tr><td colspan=2>{name}</td></tr>
    <tr><td colspan=2>{sc_hc}</td></tr>
    <tr><td colspan=2>{exp_classic}</td></tr>
    <tr><td colspan=2>/players {px}</td></tr>
    <tr><td colspan=2>LVL: {level}</td></tr>
    <tr><td colspan=2>DEATHS: {deaths}</td></tr>
    <tr><td colspan=2>RUNS: {runs}</td></tr>
    <tr><td>GOLD:</td><td>{gold}</td></tr>
    <tr><td>MF:</td><td>{mf}</td></tr>
    <tr><td>EMG:</td><td>{emg}</td></tr>
    <tr><td>ATD:</td><td>{atd}</td></tr>
    <tr><td>STR:</td><td>{str}</td></tr>
    <tr><td>DEX:</td><td>{dex}</td></tr>
    <tr><td>VIT:</td><td>{vit}</td></tr>
    <tr><td>ENE:</td><td>{ene}</td></tr>
    <tr><td>FRW:</td><td>{frw}</td></tr>
    <tr><td>FHR:</td><td>{fhr}</td></tr>
    <tr><td>FCR:</td><td>{fcr}</td></tr>
    <tr><td>IAS:</td><td>{ias}</td></tr>
    <tr><td>FIRE:</td><td>{fire}</td></tr>
    <tr><td>COLD:</td><td>{cold}</td></tr>
    <tr><td>LIGH:</td><td>{light}</td></tr>
    <tr><td>POIS:</td><td>{poison}</td></tr>
    <tr><td>NORM:</td><td>{norm_complete}</td></tr>
    <tr><td>NM:</td><td>{nm_complete}</td></tr>
    <tr><td>HELL:</td><td>{hell_complete}</td></tr>
</table>";

            try
            {
                while (true)
                {
                    // Note: The GetContext method blocks while waiting for a request. 
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;

                    string style = horizontalStyle;
                    string body = horizontalBody;
                    var res = Regex.Split(body, @"({[^}]+})");
                    var sb = new StringBuilder();
                    for (int i = 0; i < res.Length; i++)
                        sb.Append(i % 2 == 0 ? res[i] : parse(res[i]));

                    string responseString = $"<html><head><style type=\"text/css\">{defaultStyle}{style}</style></head><body>{sb.ToString()}</body></html>";


                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Http server error", e);
                listener.Stop();
            }
        }
    }
}
