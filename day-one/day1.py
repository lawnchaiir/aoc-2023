import sys

with open("input.txt") as f:
    lines = f.readlines()

def solution_one():
    calibration_values = []
    for line in lines:
        first_digit = None
        last_digit = None
        for offset in range(len(line) - 1):
            if first_digit is None:
                front = line[offset]
                if front.isdigit():
                    first_digit = front

            if last_digit is None:
                back = line[len(line) - 2 - offset] # -2 because the lines still have newline chars at the end
                if back.isdigit():
                    last_digit = back
            
            if first_digit is not None and last_digit is not None:
                calibration = int(first_digit + last_digit)
                calibration_values.append(calibration)
                break

    print(sum(calibration_values))


solution_one()