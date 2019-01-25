﻿/*
	CSVReader by Dock. (24/8/11)
	http://starfruitgames.com
 
	usage: 
	CSVReader.SplitCsvGrid(textString)
 
	returns a 2D string array. 
 
	Drag onto a gameobject for a demo of CSV parsing.
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CSVReader {

	/** 
		Example use
	*/
	// Populate a TextAsset with the csvdata (e.g. TextAsset myCsvFile)
	// then do:  CSVReader.ReadCsvData(myCsvFile)

	static public string[, ] ReadCsvData (TextAsset csv) {
		return SplitCsvGrid (csv.text);
	}

	static public string[] ReadCsvRow (TextAsset csv, int row) {
		string[, ] data = SplitCsvGrid (csv.text);

		List<string> rowData = new List<string> ();
		for (int j = 0; j < data.GetLength (0); j++) {
			rowData.Add (data[j, row]);
		}

		return rowData.ToArray ();
	}

	// splits a CSV file into a 2D string array
	static public string[, ] SplitCsvGrid (string csvText) {
		string[] lines = csvText.Split ("\n" [0]);

		// finds the max width of row
		int width = 0;
		for (int i = 0; i < lines.Length; i++) {
			string[] row = SplitCsvLine (lines[i]);
			width = Mathf.Max (width, row.Length);
		}

		// creates new 2D string grid to output to
		string[, ] outputGrid = new string[width + 1, lines.Length + 1];
		for (int y = 0; y < lines.Length; y++) {
			string[] row = SplitCsvLine (lines[y]);
			for (int x = 0; x < row.Length; x++) {
				outputGrid[x, y] = row[x];

				// This line was to replace "" with " in my output. 
				// Include or edit it as you wish.
				outputGrid[x, y] = outputGrid[x, y].Replace ("\"\"", "\"");
			}
		}

		return outputGrid;
	}

	// splits a CSV row 
	/* fixformat ignore:start */
	static public string[] SplitCsvLine (string line) {
		return (
			from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches (
				line, 
				@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
				System.Text.RegularExpressions.RegexOptions.ExplicitCapture
			) select m.Groups[1].Value
		).ToArray ();
	}
	/* fixformat ignore:end */
}