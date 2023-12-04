package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"unicode"
)

type Pos struct {
	X int
	Y int
}

func main() {

	lines := loadInput()
	sum := 0
	gears := make(map[Pos][]int)

	for lineIdx, line := range lines {
		isParsingNumber := false
		currentNumber := ""
		partAdjacent := false
		var currentGear *Pos

		for charIdx, r := range line {
			isDigit := unicode.IsDigit(r)

			// accumulate the digits as we go, and check each one for adjacent parts
			if isDigit {
				isParsingNumber = true
				currentNumber += string(r)

				if !partAdjacent {
					part, pos := getAdjacentPart(lineIdx, lines, charIdx, line)

					if part != 0 {
						partAdjacent = true

						if part == '*' {
							currentGear = &pos
						}
					}
				}
			}

			// we're at the end of a number, so flush and reset
			if (isParsingNumber && !isDigit) || charIdx == len(line)-1 {
				isParsingNumber = false

				if partAdjacent {
					if i, err := strconv.Atoi(currentNumber); err == nil {
						sum += i

						if currentGear != nil {
							gearPartNumbers, ok := gears[*currentGear]
							if !ok {
								gearPartNumbers = make([]int, 0)
							}
							gearPartNumbers = append(gearPartNumbers, i)
							gears[*currentGear] = gearPartNumbers
						}
					}
				}

				currentNumber = ""
				partAdjacent = false
				currentGear = nil
			}
		}
	}
	fmt.Println(sum)

	sum = 0
	for _, gearPartNumbers := range gears {
		if len(gearPartNumbers) == 2 {
			sum += (gearPartNumbers[0] * gearPartNumbers[1])
		}
	}
	fmt.Println(sum)
}

func getAdjacentPart(lineIdx int, lines []string, charIdx int, line string) (byte, Pos) {
	for offsetX := -1; offsetX <= 1; offsetX++ {
		x := lineIdx + offsetX

		if x < 0 || x >= len(lines) {
			continue
		}
		for offsetY := -1; offsetY <= 1; offsetY++ {
			if offsetX == 0 && offsetY == 0 {
				// don't need to check the current location
				continue
			}

			y := charIdx + offsetY

			if y < 0 || y >= len(line) {
				continue
			}

			char := lines[x][y]
			if char != '.' && !unicode.IsDigit(rune(char)) {
				return char, Pos{x, y}
			}

		}

	}
	return 0, Pos{0, 0}
}

func loadInput() []string {
	file, err := os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	scan := bufio.NewScanner(file)

	var lines []string
	for scan.Scan() {
		lines = append(lines, scan.Text())
	}
	return lines
}
