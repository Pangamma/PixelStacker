<h1>Command Line Interface (CLI) Instructions</h1> 
<p>Did you know? You can call pixelstacker from the commandline to skip the GUI.</p>

<table>
	<thead>
		<tr>
			<th>Argument</th>
			<th>Description</th>
			<th>Example</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<th>--input={filepath}</th>
			<td>Specify file name to convert</td>
			<td>--input=C:\Users\User\Pictures\PS-Ideas\blue.jpg</td>
		</tr>
		<tr>
			<th>--help</th>
			<td>Prints the help text if compiled as a console application.</td>
			<td></td>
		</tr>
		<tr>
			<th>--output=</th>
			<td>File path for output. Format of result depends on extension given for file name. (.png, .schem)</td>
			<td>--output=C:\Users\User\Pictures\output.schem</td>
		</tr>
		<tr>
			<th>--format</th>
			<td>Specify format of output directly. (SCHEM, PNG, etc)</td>
			<td>--format=png</td>
		</tr>
		<tr>
			<th>--options={optionsJson}</th>
			<td>A weird one, but also a powerful one. By default, the program will use whatever options you configured the last time you used the GUI. But if you reeeally want to override the settings you can provide the serialized options json to this argument here.</td>
		</tr>
	</tbody>
</table>
