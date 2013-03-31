#!/usr/bin/perl -w
use strict;

use constant OUTPUT_DIR => 'output/';

main();

sub main	{
	# Get the name of the file to read in
	my $csvFile  = getCsvFilename();

	# Load the contents of the file
	my $contents = loadCsvFile($csvFile) or die "Failed to load .csv file - $!";

	# Create the output directory if it doesn't exist
	mkdir(OUTPUT_DIR) unless(-d OUTPUT_DIR);

	# Produce a class file from each template
	foreach my $entry (@$contents)	{
		my $filename = createClassFile($entry);
		if($filename)	{
			print $entry->{Name} . " -> $filename\n";
		}	else	{
			print STDERR 'Failed to create class file for - ' . $entry->{Name} . "\n";
		}
	}
}

# Functions #
sub getCsvFilename	{
	if(@ARGV)	{
		return $ARGV[0];
	}	else	{
		print "Input file (.csv): ";
		my $csvFile = <STDIN>;
		chomp($csvFile);
		return $csvFile;
	}
}

sub loadCsvFile	{
	my($csvFile) = @_;

	if(open(CSV, '<', $csvFile))	{
		# First line is key names
		my $line = <CSV>;
		chomp($line);
		my @keys = split(/,/, $line);

		# Read remaining lines
		my @entries;
		while(my $line = <CSV>)	{
			chomp($line);
			next unless($line);
			my %entry;
			my $i = 0;
			foreach my $value (split(/,/, $line))	{
				$entry{$keys[$i++]} = $value;
			}
			push(@entries, \%entry);
		}

		close(CSV);
		return \@entries;
	}

	else	{
		return undef;
	}
}

sub createClassFile	{
	my($values) = @_;

	my $id         = $values->{Type};
	my $type       = $values->{Name};
	my $lcName     = lc($type);	$type =~ s/\s+//g;
	my $name       = $type . 'Block';
	my $filename   = $name . '.cs';
	my $solid      = ($values->{Solid}     =~ /^yes$/i) ? 'true' : 'false';
	my $physics    = ($values->{Physics}   =~ /^yes$/i) ? 'true' : 'false';
	my $flammable  = ($values->{Flammable} =~ /^yes$/i) ? 'true' : 'false';
	my $opacity    = int($values->{Opacity});
	my $luminance  = int($values->{Luminance});
	my $resistance = $values->{'Blast Resistance'} . 'f';
	my $meta       = ($values->{'Meta-data'} =~ /^yes$/i) ? "\n\n\t\t// TODO: Implement meta-data values" : '';
	my $nbt        = $values->{'NBT Data'} ? "\n\n\t\t// TODO: Implement NBT data for '" . $values->{'NBT Data'} . "'" : '';

	if(open(CLASS, '>', OUTPUT_DIR . $filename))	{
		print CLASS<<END_CLASS_FILE;
namespace Lapis.Blocks
{
	public class $name : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.$type; }
		}

		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		public override bool IsSolid
		{
			get { return $solid; }
		}

		/// <summary>
		/// Whether or not the block obeys physics
		/// </summary>
		public override bool Physics
		{
			get { return $physics; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return $flammable; }
		}

		/// <summary>
		/// Amount of light the block absorbs (0 is fully transparent and 15 is fully opaque)
		/// </summary>
		public override int Opacity
		{
			get { return $opacity; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override int Luminance
		{
			get { return $luminance; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return $resistance; }
		}$meta$nbt
		#endregion

		/// <summary>
		/// Creates a new $lcName block
		/// </summary>
		public $name ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new $lcName block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public $name (byte data)
			: base(data)
		{
			// ...
		}
	}
}
END_CLASS_FILE

		close(CLASS);
		return $filename;
	}

	else	{
		return undef;
	}
}