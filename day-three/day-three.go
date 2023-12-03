package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"unicode"
)

func main() {

	lines := loadInput()

	sum := 0

	for lineIdx, line := range lines {
		isParsingNumber := false
		currentNumber := ""
		partAdjacent := false
		for charIdx, r := range line {
			isDigit := unicode.IsDigit(r)

			if isDigit {
				isParsingNumber = true
				currentNumber += string(r)

				if !partAdjacent {

					if checkAdjacentParts(lineIdx, lines, charIdx, line) {
						partAdjacent = true
					}
				}
			}

			if (isParsingNumber && !isDigit) || charIdx == len(line)-1 {

				isParsingNumber = false

				if i, err := strconv.Atoi(currentNumber); err == nil {

					if partAdjacent {
						sum += i
					}
				}

				currentNumber = ""
				partAdjacent = false
			}
		}
	}
	fmt.Println(sum)
}

func checkAdjacentParts(lineIdx int, lines []string, charIdx int, line string) bool {
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
				return true
			}

		}

	}
	return false
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
